using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SbpExampleShop.Backend.Abstractions.Repositories;
using SbpExampleShop.Backend.Contracts;
using SbpExampleShop.Backend.Logic;

namespace SbpExampleShop.Backend.Controllers
{
    [Route("payment")]
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly PaymentStatusService _paymentStatusService;

        public PaymentController(PaymentService paymentService, PaymentStatusService paymentStatusService)
        {
            _paymentService = paymentService;
            _paymentStatusService = paymentStatusService;
        }

        [HttpPost("buy")]
        public async Task<ActionResult<BuyResponse>> Buy([FromBody] BuyRequest request)
        {
            var paymentInfo = await _paymentService.Buy(request.ProductId);
            return new ActionResult<BuyResponse>(new BuyResponse
            {
                QrId = paymentInfo.QrId,
                EncodedQr = paymentInfo.EncodedQr
            });
        }

        [HttpGet("status/{qrId}")]
        public async Task<ActionResult<StatusResponse>> GetStatus(string qrId)
        {
            var status = await _paymentStatusService.GetStatus(qrId);
            
            return new ActionResult<StatusResponse>( new StatusResponse
            {
                Status = Status.Success//MapStatus(status)
            });
        }

        private Status MapStatus(PaymentStatus status)
        {
            switch (status)
            {
                case PaymentStatus.Success:
                    return Status.Success;
                case PaymentStatus.Error:
                    return Status.Error;
                case PaymentStatus.Waiting:
                    return Status.Waiting;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}