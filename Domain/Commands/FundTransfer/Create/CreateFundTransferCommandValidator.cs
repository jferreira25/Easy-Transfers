using FluentValidation;

namespace Easy.Transfers.Domain.Commands.FundTransfer.Create
{
    public class CreateFundTransferCommandValidator : AbstractValidator<CreateFundTransferCommand>
    {
        public CreateFundTransferCommandValidator()
        {
            RuleFor(transfer => transfer.AccountDestination).NotEmpty().WithMessage("AccountDestination Invalid account number");
            RuleFor(transfer => transfer.AccountOrigin).NotEmpty().WithMessage("AccountDestination Invalid account number");
            RuleFor(transfer => transfer.Value).GreaterThan(0).WithMessage("value greater than zero");
        }
    }
}