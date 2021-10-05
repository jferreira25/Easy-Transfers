namespace Easy.Transfers.Domain.Commands.FundTransfer.Create
{
    public class CreateFundTransferCommandResponse
    {
        public string TransactionId { get; set; }

        public CreateFundTransferCommandResponse(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
