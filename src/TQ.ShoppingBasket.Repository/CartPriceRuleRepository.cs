using System.Threading.Tasks;
using TQ.ShoppingBasket.Model.CartPriceRule;
using TQ.ShoppingBasket.Repository.Common;

namespace TQ.ShoppingBasket.Repository
{
    public class CartPriceRuleRepository : ICartPriceRuleRepository
    {
        private readonly CartPriceRule[] _discountRules = { };

        public virtual Task<CartPriceRule[]> GetCartPriceRulesAsync()
        {
            return Task.FromResult(_discountRules);
        }
    }
}