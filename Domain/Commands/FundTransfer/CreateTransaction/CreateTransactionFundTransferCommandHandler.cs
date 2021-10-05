using AutoMapper;
using Easy.Transfers.CrossCutting.Configuration.Exceptions;
using Easy.Transfers.Domain.Entities.Mongo;
using Easy.Transfers.Domain.Enum;
using Easy.Transfers.Domain.Interfaces.MongoDb;
using Easy.Transfers.Domain.Interfaces.Services;
using Easy.Transfers.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Transfers.Domain.Commands.FundTransfer.CreateTransaction
{
    public class CreateTransactionFundTransferCommandHandler : IRequestHandler<CreateTransactionFundTransferCommand, CreateTransactionFundTransferCommandResponse>
    {
        private readonly ITestaAcesso _testaAcesso;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTransactionFundTransferCommandHandler> _logger;

        public CreateTransactionFundTransferCommandHandler(
            ITestaAcesso testaAcesso,
            ITransactionRepository transactionRepository,
            IMapper mapper,
            ILogger<CreateTransactionFundTransferCommandHandler> logger)
        {
            _testaAcesso = testaAcesso;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateTransactionFundTransferCommandResponse> Handle(CreateTransactionFundTransferCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initializing handler - CreateTransaction {request.TransactionId}");

            var transaction = await _transactionRepository.GetByTransactionIdAsync(request.TransactionId);

            try
            {
                ModifiedTransaction(transaction, TransferStatus.Processing);

                await _transactionRepository.UpdateAsync(transaction);

                var accountOrigin = await _testaAcesso.GetAccountByAccountNumber(transaction.AccountOrigin);

                var accountDestination = await _testaAcesso.GetAccountByAccountNumber(transaction.AccountDestination);

                if (accountOrigin.Balance < transaction.Value)
                {
                    transaction.MessageError = "insufficient funds";

                    _logger.LogInformation($"handler - CreateTransaction Error:{ transaction.MessageError } | {request.TransactionId}");
                }

                _logger.LogInformation($"handler - CreateTransaction initializing transfer debit transactionId: {request.TransactionId} account:{accountOrigin.AccountNumber}");

                await _testaAcesso.TransferAsync(AccountDetails(accountOrigin, TransferType.Debit, transaction));

                _logger.LogInformation($"handler - CreateTransaction end transfer debit");

                _logger.LogInformation($"handler - CreateTransaction initializing transfer credit transactionId: {request.TransactionId}  account:{accountOrigin.AccountNumber}");

                await _testaAcesso.TransferAsync(AccountDetails(accountDestination, TransferType.Credit, transaction));

                _logger.LogInformation($"handler - CreateTransaction end transfer credit");

                ModifiedTransaction(transaction, TransferStatus.Confirmed);
            }
            catch (ApiHttpCustomException ex)
            {
                transaction.MessageError = ex?.Message;

                _logger.LogError($"handler - Erro CreateTransaction transactionId:{transaction.TransactionId} | messageError:{transaction.MessageError}");

                ModifiedTransaction(transaction, TransferStatus.Error);
            }
            finally
            {
                await _transactionRepository.UpdateAsync(transaction);
            }

            return new CreateTransactionFundTransferCommandResponse();
        }

        private TransferAccount AccountDetails(Account account, TransferType transferType, Transaction transaction)
        {
            var transferAccount = _mapper.Map<TransferAccount>(account);

            transferAccount.Type = transferType;
            transferAccount.Value = transaction.Value;

            return transferAccount;
        }

        private void ModifiedTransaction(Transaction transaction, TransferStatus transferStatus)
        {
            transaction.ChangedTransaction = DateTime.Now;
            transaction.Status = transferStatus;
        }

    }
}
