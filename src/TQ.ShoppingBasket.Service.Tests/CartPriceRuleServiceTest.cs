using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TQ.ShoppingBasket.Model;
using TQ.ShoppingBasket.Model.Basket;
using TQ.ShoppingBasket.Model.CartPriceRule;
using TQ.ShoppingBasket.Repository.Common;
using TQ.ShoppingBasket.Service.Common;
using TQ.ShoppingBasket.Service.Rules;
using Xunit;

namespace TQ.ShoppingBasket.Service.Tests
{
    public class CartPriceRuleServiceTest
    {
        public CartPriceRuleServiceTest()
        {
            _fixture = new CartPriceRuleServiceFixture();
        }

        private readonly CartPriceRuleServiceFixture _fixture;

        public class CartPriceRuleServiceFixture
        {
            public Mock<ICartPriceRuleRepository> CartPriceRuleRepository;
            public Mock<IRuleFactory> RuleFactory;
            public CartPriceRuleService Target;

            public CartPriceRuleServiceFixture()
            {
                CartPriceRuleRepository = new Mock<ICartPriceRuleRepository>();
                RuleFactory = new Mock<IRuleFactory>();
                Target = new CartPriceRuleService(CartPriceRuleRepository.Object, RuleFactory.Object);
            }
        }

        [Fact]
        public async Task Should_Apply_BuyXGetYFreeRule()
        {
            var buyXGetYFreeRule = new CartPriceRule
            {
                Name = "Buy 3 Milk get the 4th for free.",
                SourceCondition = new Condition {Sku = "200202"},
                DestinationCondition = new Condition {Sku = "200202"},
                Action = new Action
                    {ActionType = ActionType.BuyXGetYFree, DiscountStep = 3, DiscountAmount = 1} //Buy 3, get 1 for free
            };
            var cartPriceRules = new[]
            {
                buyXGetYFreeRule
            };
            var basket = new Basket(101)
            {
                BasketItems = new List<BasketItem>
                    {new BasketItem(new Product {Sku = "200202", Name = "Milk", Price = 1.15m}, 4)}
            };
            _fixture.CartPriceRuleRepository.Setup(s => s.GetCartPriceRulesAsync())
                .Returns(() => Task.FromResult(cartPriceRules));
            _fixture.RuleFactory.Setup(s => s.GetRule(basket.BasketItems, buyXGetYFreeRule))
                .Returns(() => new BuyXGetYFreeRule(basket.BasketItems, buyXGetYFreeRule));

            await _fixture.Target.ApplyPriceRulesAsync(basket);

            basket.Discounts.First().Price.Should().Be(1.15m);
        }

        [Fact]
        public async Task Should_Apply_PercentOfProductDiscountRule()
        {
            var percentOfProductDiscountRule = new CartPriceRule
            {
                Name = "Buy 2 butters and get one bread at 50%",
                SourceCondition = new Condition {Sku = "303003"},
                DestinationCondition = new Condition {Sku = "404004"},
                Action = new Action
                {
                    ActionType = ActionType.PercentOfProductDiscount, DiscountStep = 2, DiscountAmount = 50
                } //Buy 2 butters, get one bread at 50%
            };
            var cartPriceRules = new[]
            {
                percentOfProductDiscountRule
            };
            var basket = new Basket(101)
            {
                BasketItems = new List<BasketItem>
                {
                    new BasketItem(new Product {Sku = "303003", Name = "Butter", Price = 0.80m}, 2),
                    new BasketItem(new Product {Sku = "404004", Name = "Bread", Price = 1.0m}, 2)
                }
            };
            _fixture.CartPriceRuleRepository.Setup(s => s.GetCartPriceRulesAsync())
                .Returns(() => Task.FromResult(cartPriceRules));
            _fixture.RuleFactory
                .Setup(s => s.GetRule(basket.BasketItems, percentOfProductDiscountRule))
                .Returns(() => new PercentOfProductDiscountRule(basket.BasketItems, percentOfProductDiscountRule));

            await _fixture.Target.ApplyPriceRulesAsync(basket);

            basket.Discounts.First().Price.Should().Be(0.50m);
        }

        [Fact]
        public void Should_Instantiate_CartPriceRuleService()
        {
            _fixture.Target.Should().NotBeNull();
        }
    }
}