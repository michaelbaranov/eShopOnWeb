using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Result;
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
    private readonly BasketBuilder _basketBuilder = new BasketBuilder();

    public AddItemToBasket()
    {
        var dbOptions = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalog_AddItemToBasket")
            .Options;
        _catalogContext = new CatalogContext(dbOptions);
        _basketRepository = new EfRepository<Basket>(_catalogContext);
    }

    [Fact]
    public async Task AddsItemToBasketSuccessfully()
    {
        var basketService = new BasketService(_basketRepository, null);
        var basket = _basketBuilder.Build();
        await basketService.AddItemToBasket(basket.BuyerId, 2, 3.40m);
        _catalogContext.SaveChanges();

        var result = await _basketRepository.GetByIdAsync(basket.Id);
        Assert.Single(result.Items);
        Assert.Contains(result.Items, item => item.CatalogItemId == 2 && item.UnitPrice == 3.40m && item.Quantity == 1);
    }
}
