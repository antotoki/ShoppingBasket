using System.Collections.Generic;
using TQ.ShoppingBasket.Model.Basket;
using TQ.ShoppingBasket.Model.CartPriceRule;

namespace TQ.ShoppingBasket.Service.Common
{
    public interface IRuleFactory
    {
        IRule GetRule(IEnumerable<BasketItem> basketItems, CartPriceRule cartPriceRule);
    }
}