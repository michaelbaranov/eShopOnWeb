using ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Repositories.BasketRepositoryTests
{
    public class AddItemToBasket : BaseAsyncRepositoryTest
    {
        private readonly IBasketService _basketService;

        public AddItemToBasket()
        {
            var dbOptions = CreateNewContextOptions();
            var serviceProvider = GetServiceProvider(dbOptions);

            _basketService = serviceProvider.GetRequiredService<IBasketService>();
        }

        [Fact]
        public async Task AddsItemToBasket()
        {
            var testBuyerId = Guid.NewGuid().ToString();
            var basket = await _basketService.GetOrCreateBasketForUser(testBuyerId);
            var testCatalogItemId = 123;
            var testQuantity = 2;

            await _basketService.AddItemToBasket(basket.Id, testCatalogItemId, testQuantity);

            var updatedBasket = await _basketService.GetBasketById(basket.Id);

            Assert.Single(updatedBasket.Items);
            Assert.Equal(testCatalogItemId, updatedBasket.Items[0].CatalogItemId);
            Assert.Equal(testQuantity, updatedBasket.Items[0].Quantity);
        }
    }
}