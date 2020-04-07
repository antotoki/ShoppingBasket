using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.ShoppingBasket.Model.Basket;

namespace TQ.ShoppingBasket.Infrastructure.Logging
{
    public interface IBasketLogger
    {
        void LogBasket(Basket basket);
    }
}
