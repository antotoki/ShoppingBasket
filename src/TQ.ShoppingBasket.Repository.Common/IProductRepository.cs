using System.Threading.Tasks;
using TQ.ShoppingBasket.Model;

namespace TQ.ShoppingBasket.Repository.Common
{
    public interface IProductRepository
    {
        Task<Product> GetProductBySkuAsync(string sku);
    }
}