using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Xunit;

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
    public void DoesNotRemovesNonEmptyBasketItems()
    {
        var basket = new Basket("testBuyerId");
        basket.AddItem(1, 10m, 1);
        basket.RemoveEmptyItems();

        Assert.Single(basket.Items);
    }
}