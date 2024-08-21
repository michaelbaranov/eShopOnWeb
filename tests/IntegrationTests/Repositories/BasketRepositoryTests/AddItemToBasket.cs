using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Infrastructure.Data;
using Xunit;

namespace Microsoft.eShopWeb.IntegrationTests.Repositories.BasketRepositoryTests;

public class AddItemToBasketTest
{
    private readonly CatalogContext _catalogContext;
    private readonly IBasketRepository _basketRepository;
    private readonly BasketService _basketService;

    public AddItemToBasketTest()
    {
        var dbOptions = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalog")
            .Options;
        _catalogContext = new CatalogContext(dbOptions);
        _basketRepository = new EfRepository<Basket>(_catalogContext);
        _basketService = new BasketService(_basketRepository, null);
    }

    [Fact]
    public async Task CanAddItemToBasket()
    {
        var basket = new Basket("testId");
        await _basketRepository.AddAsync(basket);
        await _basketService.AddItemToBasket(basket.Id, 1, 2.50m, 1);

        Assert.Equal(1, basket.Items.Count);
        var firstItem = basket.Items[0];
        Assert.Equal(1, firstItem.CatalogItemId);
        Assert.Equal(2.50m, firstItem.UnitPrice);
        Assert.Equal(1, firstItem.Quantity);
    }
}