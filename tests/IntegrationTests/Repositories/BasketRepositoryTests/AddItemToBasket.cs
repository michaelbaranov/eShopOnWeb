using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.UnitTests.Builders;
using Xunit;

namespace Microsoft.eShopWeb.IntegrationTests.Repositories.BasketRepositoryTests;

public class AddItemToBasket
{
    private readonly CatalogContext _catalogContext;
    private readonly EfRepository<Basket> _basketRepository;
    private readonly BasketBuilder BasketBuilder = new BasketBuilder();

    public AddItemToBasket()
    {
        var dbOptions = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalog")
            .Options;
        _catalogContext = new CatalogContext(dbOptions);
        _basketRepository = new EfRepository<Basket>(_catalogContext);
    }

    [Fact]
    public async Task AddsItemToBasketSuccessfully()
    {
        var basket = BasketBuilder.WithDefaultValues();
        var basketService = new BasketService(_basketRepository, null);

        await _basketRepository.AddAsync(basket);
        _catalogContext.SaveChanges();

        await basketService.AddItemToBasket(basket.Id, 2, 5);

        Assert.Single(basket.Items);
        Assert.Equal(5, basket.Items[0].Quantity);
    }
}