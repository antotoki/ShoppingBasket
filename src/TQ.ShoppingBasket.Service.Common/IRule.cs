using TQ.ShoppingBasket.Model.Basket;

namespace TQ.ShoppingBasket.Service.Common
{
    public interface IRule
    {
        Discount ApplyRule();
    }
}