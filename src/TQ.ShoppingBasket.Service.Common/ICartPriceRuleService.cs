using System.Threading.Tasks;
using TQ.ShoppingBasket.Model.Basket;

namespace TQ.ShoppingBasket.Service.Common
{
    public interface ICartPriceRuleService
    {
        Task ApplyPriceRulesAsync(Basket basket);
    }
}