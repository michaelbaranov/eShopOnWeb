using ApplicationCore.Entities.BasketAggregate;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Repositories.BasketRepositoryTests
{
    public class AddItemToBasket : BaseRepositoryTest
    {
        private readonly IBasketService _basketService;
        private readonly IAsyncRepository<Basket> _basketRepository;

        public AddItemToBasket()
        {
            _basketRepository = GetRequiredService<IAsyncRepository<Basket>>();
            _basketService = GetRequiredService<IBasketService>();
        }

        [Fact]
        public async Task AddsItemToEmptyBasket()
        {
            var testBasket = await _basketRepository.AddAsync(new Basket("TestUserId"));

            await _basketService.AddItemToBasket(testBasket.Id, 1, 10.00m, 1);

            var updatedBasket = await _basketRepository.GetByIdAsync(testBasket.Id, basketSpec);

            Assert.Single(updatedBasket.Items);
            Assert.Equal(1, updatedBasket.Items[0].CatalogItemId);
            Assert.Equal(10.00m, updatedBasket.Items[0].UnitPrice);
            Assert.Equal(1, updatedBasket.Items[0].Quantity);
        }
    }
}