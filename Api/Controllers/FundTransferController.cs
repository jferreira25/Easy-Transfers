using Easy.Transfers.Controllers;
using Easy.Transfers.Domain.Commands.FundTransfer.Create;
using Easy.Transfers.Domain.Queries.FundTransfer.GetFundTransferByTransactionId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Easy.Transfers.Admin.Controllers
{
    [Route("fund-transfer")]
    public class FundTransferController : BaseController<FundTransferController>
    {
        public FundTransferController(IMediator mediatorService, ILogger<BaseController<FundTransferController>> logger) : base(mediatorService, logger)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateFundTransferAsync([FromBody] CreateFundTransferCommand command)
        {
            return await GenerateResponseAsync(async () => await MediatorService.Send(command));
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetFundTransferAsync(string transactionId)
        {
            return await GenerateResponseAsync(async () => await MediatorService.Send(new GetFundTransferByTransactionIdQuery(transactionId)));
        }
    }
}
