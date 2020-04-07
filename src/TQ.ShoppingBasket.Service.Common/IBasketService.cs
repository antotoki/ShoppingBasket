using System.Threading.Tasks;

namespace TQ.ShoppingBasket.Service.Common
{
    public interface IBasketService
    {
        Task AddItemToBasketAsync(int sessionId, string sku, int quantity);
        Task RemoveItemFromBasketAsync(int sessionId, string sku);
        Task UpdateExistingBasketItemAsync(int sessionId, string sku, int quantity);
        Task<decimal> GetTotalSumAsync(int sessionId);
    }
}