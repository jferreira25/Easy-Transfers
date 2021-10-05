using AutoMapper;
using Easy.Transfers.Domain.Commands.FundTransfer.CreateTransaction;
using Easy.Transfers.Domain.Publishers;
using MediatR;
using System.Threading.Tasks;

namespace Easy.Transfers.Infrastructure.Subscriber
{
    public class TransactionConsumer: Consumer<TransactionEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public TransactionConsumer(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task ConsumeAsync(TransactionEvent transactionEvent)
        {
            var createTransactionFundTransferCommand = _mapper.Map<CreateTransactionFundTransferCommand>(transactionEvent);

            await _mediator.Send(createTransactionFundTransferCommand);
        }
    }
}
