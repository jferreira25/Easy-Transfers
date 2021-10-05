using AutoMapper;
using Easy.Transfers.CrossCutting.Configuration.Exceptions;
using Easy.Transfers.Domain.Entities.Mongo;
using Easy.Transfers.Domain.Interfaces.MongoDb;
using Easy.Transfers.Domain.Interfaces.Services;
using Easy.Transfers.Domain.Publishers;
using Easy.Transfers.Infrastructure.Publisher;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Transfers.Domain.Commands.FundTransfer.Create
{
    public class CreateFundTransferCommandHandler : IRequestHandler<CreateFundTransferCommand, CreateFundTransferCommandResponse>
    {
        private readonly ITestaAcesso _testaAcesso;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly TransactionPublisher<TransactionEvent> _transactionPublisher;
        private readonly ILogger<CreateFundTransferCommandHandler> _logger;

        public CreateFundTransferCommandHandler(
            ITestaAcesso testaAcesso,
            ITransactionRepository transactionRepository,
            IMapper mapper,
            TransactionPublisher<TransactionEvent> transactionPublisher,
            ILogger<CreateFundTransferCommandHandler> logger)
        {
            _testaAcesso = testaAcesso;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _transactionPublisher = transactionPublisher;
            _logger = logger;
        }

        public async Task<CreateFundTransferCommandResponse> Handle(CreateFundTransferCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initializing handler - CreateFundTransfer");

            var accountOrigin = await _testaAcesso.GetAccountByAccountNumber(request.AccountOrigin);

            var accountDestination = await _testaAcesso.GetAccountByAccountNumber(request.AccountDestination);

            if (accountOrigin.AccountNumber == accountDestination.AccountNumber)
                throw new ApiHttpCustomException($"Not is posible to transfer same account", System.Net.HttpStatusCode.BadRequest, null);

            var transfer = _mapper.Map<Transaction>(request);

            transfer.Status = Enum.TransferStatus.InQueue;

            await _transactionRepository.CreateAsync(transfer);

            var transferEvent = _mapper.Map<TransactionEvent>(transfer);

            _logger.LogInformation($" Send message rabbitMq handler - CreateFundTransfer transverevent:{JsonConvert.SerializeObject(transferEvent) }");

            await _transactionPublisher.SendMessage(transferEvent);

            _logger.LogInformation($"End handler - CreateFundTransfer sucessfully |TransactionId:{transferEvent.TransactionId}");
            return new CreateFundTransferCommandResponse(transfer.TransactionId);
        }
    }
}
