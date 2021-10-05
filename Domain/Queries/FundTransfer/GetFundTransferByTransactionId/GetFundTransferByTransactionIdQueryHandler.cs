using AutoMapper;
using Easy.Transfers.Domain.Interfaces.MongoDb;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Transfers.Domain.Queries.FundTransfer.GetFundTransferByTransactionId
{
    public class GetFundTransferByTransactionIdQueryHandler : IRequestHandler<GetFundTransferByTransactionIdQuery, GetFundTransferByTransactionIdQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<GetFundTransferByTransactionIdQueryHandler> _logger;

        public GetFundTransferByTransactionIdQueryHandler(
            IMapper mapper,
            ITransactionRepository transactionRepository,
            ILogger<GetFundTransferByTransactionIdQueryHandler> logger)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task<GetFundTransferByTransactionIdQueryResponse> Handle(GetFundTransferByTransactionIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initializing handler - GetFundTransfer {request.TransactionId}");

            var transfer = await _transactionRepository.GetByTransactionIdAsync(request.TransactionId);

            _logger.LogInformation($"End handler - GetFundTransfer {request.TransactionId} exists:{transfer != null}");

            return _mapper.Map<GetFundTransferByTransactionIdQueryResponse>(transfer);
        }
    }
}
