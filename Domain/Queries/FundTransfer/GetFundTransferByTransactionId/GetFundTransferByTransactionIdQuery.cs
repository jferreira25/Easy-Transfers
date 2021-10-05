using MediatR;

namespace Easy.Transfers.Domain.Queries.FundTransfer.GetFundTransferByTransactionId
{
    public class GetFundTransferByTransactionIdQuery : IRequest<GetFundTransferByTransactionIdQueryResponse>
    {
        public GetFundTransferByTransactionIdQuery(string transactionId)
        {
            TransactionId = transactionId;
        }

        public string TransactionId { get; set; }
    }
}
