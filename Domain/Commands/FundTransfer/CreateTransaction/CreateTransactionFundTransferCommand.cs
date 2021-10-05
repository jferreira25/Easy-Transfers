using MediatR;

namespace Easy.Transfers.Domain.Commands.FundTransfer.CreateTransaction
{
    public class CreateTransactionFundTransferCommand : IRequest<CreateTransactionFundTransferCommandResponse>
    {
        public string TransactionId { get; set; }
    }
}
