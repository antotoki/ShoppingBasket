using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TQ.ShoppingBasket.Common.Logging;
using TQ.ShoppingBasket.Infrastructure.Logging;
using TQ.ShoppingBasket.Model;
using TQ.ShoppingBasket.Model.Basket;
using TQ.ShoppingBasket.Repository.Common;
using TQ.ShoppingBasket.Service.Common;
using Xunit;

namespace TQ.ShoppingBasket.Service.Tests
{
    public class BasketServiceTest
    {
        public BasketServiceTest()
        {
            _fixture = new BasketServiceFixture();
        }

        private readonly BasketServiceFixture _fixture;

        public class BasketServiceFixture
        {
            public Mock<IBasketRepository> BasketRepository;
            public Mock<ICartPriceRuleService> CartPriceRuleService;
            public IBasketLogger BasketLogger;
            public Mock<IProductRepository> ProductRepository;
            public BasketService Target;

            public BasketServiceFixture()
            {
                BasketRepository = new Mock<IBasketRepository>();
                CartPriceRuleService = new Mock<ICartPriceRuleService>();
                BasketLogger = new BasketLogger(new Logger());
                ProductRepository = new Mock<IProductRepository>();
                Target = new BasketService(BasketRepository.Object, ProductRepository.Object,
                    CartPriceRuleService.Object, BasketLogger);
            }
        }

        [Fact]
        public async Task Should_Calculate_TotalSum_With_Discounts()
        {
            #region Butter and Bread case

            var butterAndBreadBasket = new Basket(101)
            {
                BasketItems = new List<BasketItem>
                {
                    new BasketItem(new Product {Sku = "303003", Name = "Butter", Price = 0.80m}, 2),
                    new BasketItem(new Product {Sku = "404004", Name = "Bread", Price = 1.0m}, 2)
                },
                Discounts = new List<Discount>
                    {new Discount {Name = "Buy 2 butters and get one bread at 50%", Price = 0.5m}}
            };

            _fixture.BasketRepository.Setup(s => s.GetBasketBySessionIdAsync(101))
                .Returns(() => Task.FromResult(butterAndBreadBasket));
            var butterAndBreadBasketTotalSum = await _fixture.Target.GetTotalSumAsync(101);

            butterAndBreadBasketTotalSum.Should().Be(3.10m);

            #endregion butterAndBreadBasket

            #region 4 Milks case

            var fourMilksBasket = new Basket(101)
            {
                BasketItems = new List<BasketItem>
                {
                    new BasketItem(new Product {Sku = "101", Name = "Milk", Price = 1.15m}, 4)
                },
                Discounts = new List<Discount> {new Discount {Name = "Buy 3 Milk get the 4th for free.", Price = 1.15m}}
            };
            _fixture.BasketRepository.Setup(s => s.GetBasketBySessionIdAsync(101))
                .Returns(() => Task.FromResult(fourMilksBasket));
            var fourMilksTotalSum = await _fixture.Target.GetTotalSumAsync(101);

            fourMilksTotalSum.Should().Be(3.45m);

            #endregion

            #region 2 Butter, 1 Bread and 8 milks

            var complexBasket = new Basket(101)
            {
                BasketItems = new List<BasketItem>
                {
                    new BasketItem(new Product {Sku = "303003", Name = "Butter", Price = 0.80m}, 2),
                    new BasketItem(new Product {Sku = "404004", Name = "Bread", Price = 1.0m}, 1),
                    new BasketItem(new Product {Sku = "101", Name = "Milk", Price = 1.15m}, 8)
                },
                Discounts = new List<Discount>
                {
                    new Discount {Name = "Buy 3 Milk get the 4th for free.", Price = 2.30m},
                    new Discount {Name = "Buy 2 butters and get one bread at 50%", Price = 0.5m}
                }
            };
            _fixture.BasketRepository.Setup(s => s.GetBasketBySessionIdAsync(101))
                .Returns(() => Task.FromResult(complexBasket));
            var complexTotalSum = await _fixture.Target.GetTotalSumAsync(101);
            complexTotalSum.Should().Be(9.0m);

            #endregion
        }

        [Fact]
        public async Task Should_Calculate_TotalSum_Without_Discounts()
        {
            var basket = new Basket(101)
            {
                BasketItems = new List<BasketItem>
                {
                    new BasketItem(new Product {Sku = "303003", Name = "Butter", Price = 0.80m}, 1),
                    new BasketItem(new Product {Sku = "404004", Name = "Bread", Price = 1.0m}, 1),
                    new BasketItem(new Product {Sku = "101", Name = "Milk", Price = 1.15m}, 1)
                }
            };
            _fixture.BasketRepository.Setup(s => s.GetBasketBySessionIdAsync(101))
                .Returns(() => Task.FromResult(basket));

            var totalSum = await _fixture.Target.GetTotalSumAsync(101);
            totalSum.Should().Be(2.95m);
        }

        [Fact]
        public void Should_Instantiate_BasketService()
        {
            _fixture.Target.Should().NotBeNull();
        }
    }
}