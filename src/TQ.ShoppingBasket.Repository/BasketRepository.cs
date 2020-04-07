using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.ShoppingBasket.Model.Basket;
using TQ.ShoppingBasket.Repository.Common;

namespace TQ.ShoppingBasket.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly List<Basket>
            _basketStorage = new List<Basket>(); //simulate database persistence.

        private readonly IProductRepository productRepository;

        public BasketRepository(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public virtual async Task<bool> AddItemToBasketAsync(int sessionId, string sku, int quantity)
        {
            var existingBasket = _basketStorage.First(b => b.SessionId == sessionId);
            var product = await productRepository.GetProductBySkuAsync(sku);
            existingBasket.BasketItems.Add(new BasketItem(product, quantity));
            return true;
        }

        public virtual Task<Basket> GetBasketBySessionIdAsync(int sessionId)
        {
            var basket = _basketStorage.FirstOrDefault(b => b.SessionId == sessionId);
            return Task.FromResult(basket);
        }

        public virtual Task<Basket> CreateBasketModelAsync(int sessionId)
        {
            var basketModel = new Basket(sessionId);
            return Task.FromResult(basketModel);
        }

        public virtual Task InsertBasketAsync(Basket basket)
        {
            _basketStorage.Add(basket);
            return Task.CompletedTask;
        }

        public virtual Task<bool> ItemAlreadyExistsInBasketAsync(int sessionId, string sku)
        {
            var basket = _basketStorage.FirstOrDefault(b => b.SessionId == sessionId);
            if (basket != null)
                return Task.FromResult(basket.BasketItems.Any(basketItem => basketItem.Product.Sku == sku));

            return Task.FromResult(false);
        }

        public virtual Task UpdateExistingBasketItemAsync(int sessionId, string sku, int quantity)
        {
            var existingBasket = _basketStorage.First(b => b.SessionId == sessionId);
            var basketItem = existingBasket.BasketItems.First(bi => bi.Product.Sku == sku);
            basketItem.Quantity = quantity;
            return Task.CompletedTask;
        }

        public virtual Task RemoveBasketItemAsync(int sessionId, string sku)
        {
            var existingBasket = _basketStorage.First(b => b.SessionId == sessionId);
            existingBasket.BasketItems.RemoveAll(bi => bi.Product.Sku == sku);
            return Task.CompletedTask;
        }
    }
}