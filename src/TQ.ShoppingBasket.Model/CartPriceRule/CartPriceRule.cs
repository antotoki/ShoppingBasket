namespace TQ.ShoppingBasket.Model.CartPriceRule
{
    public class CartPriceRule
    {
        public string Name { get; set; }
        public Condition SourceCondition { get; set; }
        public Action Action { get; set; }
        public Condition DestinationCondition { get; set; }
    }
}