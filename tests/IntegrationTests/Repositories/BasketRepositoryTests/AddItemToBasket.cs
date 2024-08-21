using System;
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
    private readonly IBasketService _basketService;
    private readonly BasketBuilder _basketBuilder = new BasketBuilder();

    public AddItemToBasket()
    {
        var dbOptions = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalog")
            .Options;
        _catalogContext = new CatalogContext(dbOptions);
        var basketRepository = new EfRepository<Basket>(_catalogContext);
        _basketService = new BasketService(basketRepository, null);
    }

    [Fact]
    public async Task AddsItemToBasketSuccessfully()
    {
        var basket = _basketBuilder.Build();
        await _basketService.AddItemToBasket(basket.Id, 2, 5);
        await _catalogContext.SaveChangesAsync();

        var retrievedBasket = await _catalogContext.Baskets.Include(b => b.Items).FirstOrDefaultAsync(b => b.Id == basket.Id);
        Assert.NotNull(retrievedBasket);
        Assert.Single(retrievedBasket.Items);
        Assert.Equal(2, retrievedBasket.Items[0].CatalogItemId);
        Assert.Equal(5, retrievedBasket.Items[0].Quantity);
    }
}