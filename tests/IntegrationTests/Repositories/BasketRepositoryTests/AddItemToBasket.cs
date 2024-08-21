using ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.eShopWeb.IntegrationTests.Repositories.BasketRepositoryTests
{
    public class AddItemToBasket : BaseTest, IClassFixture<DatabaseFixture>
    {
        private readonly IBasketService _basketService;

        public AddItemToBasket(DatabaseFixture fixture) : base(fixture)
        {
            _basketService = Services.GetService<IBasketService>();
        }

        [Fact]
        public async Task ExecutesSuccessfully()
        {
            var testBasketId = 1;
            var testCatalogItemId = 2;
            var testQuantity = 1;

            await _basketService.AddItemToBasket(testBasketId, testCatalogItemId, testQuantity);

            var basket = await GetBasketByIdAsync(testBasketId);
            Assert.Contains(basket.Items, item => item.CatalogItemId == testCatalogItemId && item.Quantity == testQuantity);
        }

        private async Task<Basket> GetBasketByIdAsync(int basketId)
        {
            var repo = Services.GetService<IRepository<Basket>>();
            return await repo.GetByIdAsync(basketId);
        }
    }
}