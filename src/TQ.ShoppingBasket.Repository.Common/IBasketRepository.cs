using System.Threading.Tasks;
using TQ.ShoppingBasket.Model.Basket;

namespace TQ.ShoppingBasket.Repository.Common
{
    public interface IBasketRepository
    {
        Task<bool> AddItemToBasketAsync(int sessionId, string sku, int quantity);
        Task<Basket> GetBasketBySessionIdAsync(int sessionId);
        Task<Basket> CreateBasketModelAsync(int sessionId);
        Task InsertBasketAsync(Basket basket);
        Task<bool> ItemAlreadyExistsInBasketAsync(int sessionId, string sku);
        Task UpdateExistingBasketItemAsync(int sessionId, string sku, int quantity);
        Task RemoveBasketItemAsync(int sessionId, string sku);
    }
}