using System.Collections.Generic;

namespace TQ.ShoppingBasket.Model.Basket
{
    public class Basket
    {
        public Basket(int sessionId)
        {
            SessionId = sessionId;
            BasketItems = new List<BasketItem>();
            Discounts = new List<Discount>();
        }

        public int SessionId { get; }
        public List<BasketItem> BasketItems { get; set; }
        public List<Discount> Discounts { get; set; }
        public decimal TotalSum { get; set; }
    }
}