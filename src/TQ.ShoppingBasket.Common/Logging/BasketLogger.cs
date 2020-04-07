using System.Collections.Generic;
using System.Linq;
using TQ.ShoppingBasket.Common.Logging;
using TQ.ShoppingBasket.Model.Basket;

namespace TQ.ShoppingBasket.Infrastructure.Logging
{
    public class BasketLogger : IBasketLogger
    {
        private readonly ILogger _logger;

        public BasketLogger(ILogger logger)
        {
            _logger = logger;
        }
        private void LogProductInformation(IEnumerable<BasketItem> basketItems)
        {
            var productsInfo = basketItems.Select(basketItem =>
                $"Product: {basketItem.Product.Name}, Price: ${basketItem.Product.Price}, Quantity: {basketItem.Quantity}");
            foreach (var productInfo in productsInfo) _logger.LogInfo(productInfo);
        }

        private void LogDiscounts(IEnumerable<Discount> discounts)
        {
            var discountsInfo = discounts.Select(discount => $"Discount: {discount.Name} - ${discount.Price}");
            foreach (var discountInfo in discountsInfo) _logger.LogInfo(discountInfo);
        }

        private void  LogTotalSumPrice(decimal totalSum)
        {
            _logger.LogInfo($"Total Sum Price: ${totalSum}");
        }

        public void LogBasket(Basket basket)
        {
            LogProductInformation(basket.BasketItems);
            LogDiscounts(basket.Discounts);
            LogTotalSumPrice(basket.TotalSum);
        }
    }
}