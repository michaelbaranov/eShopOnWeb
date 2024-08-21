using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Xunit;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.BasketTests;

public class BasketRemoveEmptyItems
{
    private readonly int _testCatalogItemId = 123;
    private readonly decimal _testUnitPrice = 1.23m;
    private readonly string _buyerId = "Test buyerId";

    [Fact]
    public void RemovesEmptyBasketItems()
    {
        var basket = new Basket(_buyerId);
        basket.AddItem(_testCatalogItemId, _testUnitPrice, 0);
        basket.RemoveEmptyItems();

        Assert.Equal(0, basket.Items.Count);
    }

    [Fact]
    public async Task DoesNotRemovesNonEmptyBasketItems()
    {
        var options = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: "TestDB_RemoveEmpty")
            .Options;

        await using var catalogContext = new CatalogContext(options);
        IBasketRepository basketRepository = new EfRepository<Basket>(catalogContext);
        var basketService = new BasketService(basketRepository, null);

        var basketId = 1;
        var basket = new Basket("test@example.com");
        basket.AddItem(1, 2.50m, 1);
        await basketRepository.AddAsync(basket);

        await basketService.RemoveEmptyItems(basketId);

        var updatedBasket = await basketRepository.GetByIdWithItemsAsync(basketId);
        Assert.Single(updatedBasket.Items);
    }
}
```