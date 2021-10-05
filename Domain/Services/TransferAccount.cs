using Easy.Transfers.Domain.Enum;

namespace Easy.Transfers.Domain.Services
{
    public class TransferAccount
    {
        public string AccountNumber { get; set; }
        public decimal Value { get; set; }
        public TransferType Type { get; set; }
    }
}
