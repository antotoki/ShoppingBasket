using System.Threading.Tasks;
using TQ.ShoppingBasket.Model.Basket;
using TQ.ShoppingBasket.Model.CartPriceRule;
using TQ.ShoppingBasket.Repository.Common;
using TQ.ShoppingBasket.Service.Common;

namespace TQ.ShoppingBasket.Service
{
    public class CartPriceRuleService : ICartPriceRuleService
    {
        private readonly ICartPriceRuleRepository _cartPriceRuleRepository;
        private readonly IRuleFactory _ruleFactory;

        public CartPriceRuleService(ICartPriceRuleRepository cartPriceRuleRepository, IRuleFactory ruleFactory)
        {
            _cartPriceRuleRepository = cartPriceRuleRepository;
            _ruleFactory = ruleFactory;
        }

        public virtual async Task ApplyPriceRulesAsync(Basket basket)
        {
            var cartPriceRules = await _cartPriceRuleRepository.GetCartPriceRulesAsync();
            foreach (var cartPriceRule in cartPriceRules)
            {
                var discount = _ruleFactory
                                .GetRule(basket.BasketItems, cartPriceRule)
                                .ApplyRule();

                if (discount != null) basket.Discounts.Add(discount);
            }
        }
    }
}