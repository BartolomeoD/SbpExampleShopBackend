using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SbpExampleShop.Backend.Contracts;

namespace SbpExampleShop.Backend.Controllers
{
    [Route("payment")]
    public class PaymentController : Controller
    {
        [HttpPost("buy")]
        public async Task<ActionResult<BuyResponse>> Buy([FromBody] BuyRequest request)
        {
            return new ActionResult<BuyResponse>(new BuyResponse
            {
                EncodedQr = "asdasdas"
            });
        }
        
    }
}