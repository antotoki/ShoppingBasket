using System.Collections.Generic;
using System.Linq;
using TQ.ShoppingBasket.Model.Basket;
using TQ.ShoppingBasket.Model.CartPriceRule;
using TQ.ShoppingBasket.Service.Common;

namespace TQ.ShoppingBasket.Service.Rules
{
    public class PercentOfProductDiscountRule : IRule
    {
        private readonly IEnumerable<BasketItem> _basketItems;
        private readonly CartPriceRule _cartPriceRule;

        public PercentOfProductDiscountRule(IEnumerable<BasketItem> basketItems,
            CartPriceRule cartPriceRule)
        {
            _basketItems = basketItems;
            _cartPriceRule = cartPriceRule;
        }

        public Discount ApplyRule()
        {
            var sourceProduct = _basketItems.FirstOrDefault(basketItem =>
                _cartPriceRule.SourceCondition.Sku == basketItem.Product.Sku &&
                basketItem.Quantity >= _cartPriceRule.Action.DiscountStep);
            if (sourceProduct != null)
            {
                var productToApplyDiscount = _basketItems.FirstOrDefault(basketItem =>
                    basketItem.Product.Sku == _cartPriceRule.DestinationCondition.Sku);
                if (productToApplyDiscount != null)
                {
                    var cartPriceRuleQuotient = sourceProduct.Quantity / _cartPriceRule.Action.DiscountStep;
                    var rulesToApplyCount = cartPriceRuleQuotient > productToApplyDiscount.Quantity
                        ? productToApplyDiscount.Quantity
                        : cartPriceRuleQuotient;
                    var priceDiscount = productToApplyDiscount.Product.Price *
                                        (_cartPriceRule.Action.DiscountAmount / 100);
                    var discountSum = priceDiscount * rulesToApplyCount;
                    return new Discount {Name = _cartPriceRule.Name, Price = discountSum};
                }
            }

            return default;
        }
    }
}