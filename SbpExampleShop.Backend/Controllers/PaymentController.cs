using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SbpExampleShop.Backend.Contracts;
using SbpExampleShop.Backend.Logic;

namespace SbpExampleShop.Backend.Controllers
{
    [Route("payment")]
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
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
    }
}