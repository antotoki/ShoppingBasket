namespace TQ.ShoppingBasket.Model.CartPriceRule
{
    public class Action
    {
        public ActionType ActionType { get; set; }
        public decimal DiscountAmount { get; set; }
        public int DiscountStep { get; set; }
    }
}