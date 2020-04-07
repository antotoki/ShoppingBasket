using System;
using System.Collections.Generic;
using TQ.ShoppingBasket.Model.Basket;
using TQ.ShoppingBasket.Model.CartPriceRule;
using TQ.ShoppingBasket.Service.Common;

namespace TQ.ShoppingBasket.Service.Rules
{
    public class RuleFactory : IRuleFactory
    {
        public IRule GetRule(IEnumerable<BasketItem> basketItems, CartPriceRule cartPriceRule)
        {
            return (IRule)Activator.CreateInstance(Type.GetType($"TQ.ShoppingBasket.Service.Rules.{cartPriceRule.Action.ActionType}Rule"),
                new object[] {basketItems, cartPriceRule});
        }
    }
}