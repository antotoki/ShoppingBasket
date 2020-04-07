using System.Collections.Generic;
using System.Linq;
using TQ.ShoppingBasket.Model.Basket;
using TQ.ShoppingBasket.Model.CartPriceRule;
using TQ.ShoppingBasket.Service.Common;

namespace TQ.ShoppingBasket.Service.Rules
{
    public class BuyXGetYFreeRule : IRule
    {
        private readonly IEnumerable<BasketItem> _basketItems;
        private readonly CartPriceRule _cartPriceRule;

        public BuyXGetYFreeRule(IEnumerable<BasketItem> basketItems,
            CartPriceRule cartPriceRule)
        {
            _basketItems = basketItems;
            _cartPriceRule = cartPriceRule;
        }

        public Discount ApplyRule()
        {
            var sourceProduct = _basketItems.FirstOrDefault(basketItem =>
                _cartPriceRule.SourceCondition.Sku == basketItem.Product.Sku &&
                basketItem.Quantity > _cartPriceRule.Action.DiscountStep);
            if (sourceProduct != null)
            {
                var productToApplyDiscount = _basketItems.FirstOrDefault(basketItem =>
                    basketItem.Product.Sku == _cartPriceRule.DestinationCondition.Sku);
                if (productToApplyDiscount != null)
                {
                    var cartPriceRuleQuotient = (_cartPriceRule.Action.DiscountStep + 1) / sourceProduct.Quantity;
                    var rulesToApplyCount = cartPriceRuleQuotient > productToApplyDiscount.Quantity
                        ? productToApplyDiscount.Quantity
                        : cartPriceRuleQuotient;
                    var discountSum = productToApplyDiscount.Product.Price * rulesToApplyCount;
                    return new Discount {Name = _cartPriceRule.Name, Price = discountSum};
                }
            }

            return default;
        }
    }
}