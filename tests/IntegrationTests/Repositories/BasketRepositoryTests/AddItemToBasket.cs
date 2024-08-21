using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Infrastructure.Data;
using Xunit;

namespace Microsoft.eShopWeb.IntegrationTests.Repositories.BasketRepositoryTests;

public class AddItemToBasket
{
    private readonly CatalogContext _catalogContext;
    private readonly IBasketRepository _basketRepository;
    private readonly BasketService _basketService;

    public AddItemToBasket()
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
        var testBasketId = 1;
        var testCatalogItemId = 2;
        var testQuantity = 3;

        var basket = new Basket("testuser");
        await _basketRepository.AddAsync(basket);
        _catalogContext.SaveChanges();

        await _basketService.AddItemToBasket(testBasketId, testCatalogItemId, testQuantity);

        basket = await _basketRepository.GetByIdWithItemsAsync(testBasketId);
        var item = basket.Items.Find(i => i.CatalogItemId == testCatalogItemId);

        Assert.NotNull(item);
        Assert.Equal(testQuantity, item.Quantity);
    }
}