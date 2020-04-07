using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.ShoppingBasket.Infrastructure.Logging;
using TQ.ShoppingBasket.Model.Basket;
using TQ.ShoppingBasket.Repository.Common;
using TQ.ShoppingBasket.Service.Common;

namespace TQ.ShoppingBasket.Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ICartPriceRuleService _cartPriceRuleService;
        private readonly IBasketLogger _basketLogger;
        private readonly IProductRepository _productRepository;

        public BasketService(
            IBasketRepository basketRepository,
            IProductRepository productRepository,
            ICartPriceRuleService cartPriceRuleService,
            IBasketLogger basketLogger)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _cartPriceRuleService = cartPriceRuleService;
            _basketLogger = basketLogger;
        }

        public virtual async Task AddItemToBasketAsync(int sessionId, string sku, int quantity)
        {
            var existingBasket = await _basketRepository.GetBasketBySessionIdAsync(sessionId);
            if (existingBasket == null)
            {
                var newBasket = await _basketRepository.CreateBasketModelAsync(sessionId);
                var product = await _productRepository.GetProductBySkuAsync(sku);
                newBasket.BasketItems = new List<BasketItem> {new BasketItem(product, quantity)};
                await _basketRepository.InsertBasketAsync(newBasket);
            }
            else
            {
                if (!await _basketRepository.ItemAlreadyExistsInBasketAsync(sessionId, sku))
                    await _basketRepository.AddItemToBasketAsync(sessionId, sku, quantity);
                else
                    await UpdateExistingBasketItemAsync(sessionId, sku, quantity);
            }
        }

        public virtual async Task RemoveItemFromBasketAsync(int sessionId, string sku)
        {
            await _basketRepository.RemoveBasketItemAsync(sessionId, sku);
        }

        public virtual async Task UpdateExistingBasketItemAsync(int sessionId, string sku, int quantity)
        {
            await _basketRepository.UpdateExistingBasketItemAsync(sessionId, sku, quantity);
        }

        public virtual async Task<decimal> GetTotalSumAsync(int sessionId)
        {
            var existingBasket = await _basketRepository.GetBasketBySessionIdAsync(sessionId);
            await _cartPriceRuleService.ApplyPriceRulesAsync(existingBasket);
            var totalSum = existingBasket.BasketItems.Sum(basketItem => basketItem.Product.Price * basketItem.Quantity);
            var discountSum = existingBasket.Discounts.Sum(discount => discount.Price);
            existingBasket.TotalSum = totalSum - discountSum;
            _basketLogger.LogBasket(existingBasket);
            return existingBasket.TotalSum;
        }
    }
}