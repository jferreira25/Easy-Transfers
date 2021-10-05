using MediatR;

namespace Easy.Transfers.Domain.Commands.FundTransfer.Create
{
    public class CreateFundTransferCommand : IRequest<CreateFundTransferCommandResponse>

    {
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public decimal Value { get; set; }
          
    }
}
