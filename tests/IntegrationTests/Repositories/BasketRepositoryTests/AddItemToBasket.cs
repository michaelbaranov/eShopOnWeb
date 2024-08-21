using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using src.Infrastructure.Data;
using src.ApplicationCore.Entities.BasketAggregate;
using src.ApplicationCore.Interfaces;

namespace tests.IntegrationTests.Repositories.BasketRepositoryTests
{
    public class AddItemToBasket : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AddItemToBasket(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddsItemToBasketSuccessfully()
        {
            var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var basketRepo = scope.ServiceProvider.GetRequiredService<IRepository<Basket>>();
                var basket = new Basket("TestUserId");
                await basketRepo.AddAsync(basket);

                var basketService = scope.ServiceProvider.GetRequiredService<IBasketService>();
                await basketService.AddItemToBasket(basket.Id, 1, 1.0m);

                var updatedBasket = await basketRepo.GetByIdWithSpecAsync(basket.Id, new BasketWithItemsSpecification(basket.Id));
                Assert.Single(updatedBasket.Items);
            }
        }
    }
}