using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SbpExampleShop.Backend.Abstractions.Repositories;
using SbpExampleShop.Backend.Models;

namespace SbpExampleShop.Backend.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _productsList = new List<Product>()
        {
            new Product
            {
                id = 1,
                Name = "Левый кросовок",
                Description = "Очень удобный левый кросовок, правый был утерян во время прохождения " +
                              "Тествого Испытания Взаимодействия.",
                Price = 300
            },
        };

        public async Task<IEnumerable<Product>> GetProductsList()
        {
            return _productsList;
        }

        public async Task<Product> Get(long productId)
        {
            return _productsList.FirstOrDefault(product => product.id == productId);
        }
    }
}