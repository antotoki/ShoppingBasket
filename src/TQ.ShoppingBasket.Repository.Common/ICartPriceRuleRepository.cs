using System.Threading.Tasks;
using TQ.ShoppingBasket.Model.CartPriceRule;

namespace TQ.ShoppingBasket.Repository.Common
{
    public interface ICartPriceRuleRepository
    {
        Task<CartPriceRule[]> GetCartPriceRulesAsync();
    }
}