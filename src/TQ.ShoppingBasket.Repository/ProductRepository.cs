using System.Linq;
using System.Threading.Tasks;
using TQ.ShoppingBasket.Model;
using TQ.ShoppingBasket.Repository.Common;

namespace TQ.ShoppingBasket.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Product[] _products = { };

        public virtual Task<Product> GetProductBySkuAsync(string sku)
        {
            var product = _products.FirstOrDefault(p => p.Sku == sku);
            return Task.FromResult(product);
        }
    }
}