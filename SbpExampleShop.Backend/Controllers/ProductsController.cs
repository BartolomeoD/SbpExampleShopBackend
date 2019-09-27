using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SbpExampleShop.Backend.Abstractions.Repositories;

namespace SbpExampleShop.Backend.Controllers
{
    [Route("product")]   
    public class ProductsController : Controller
    {  
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("list")]
        public async Task<ActionResult> Get()
        {
            return Ok(await _productRepository.GetProductsList());
        }
    }
}