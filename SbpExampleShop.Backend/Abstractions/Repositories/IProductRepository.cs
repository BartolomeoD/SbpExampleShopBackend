using System.Collections.Generic;
using System.Threading.Tasks;
using SbpExampleShop.Backend.Models;

namespace SbpExampleShop.Backend.Abstractions.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsList();

        Task<Product> Get(long productId);
    }
}