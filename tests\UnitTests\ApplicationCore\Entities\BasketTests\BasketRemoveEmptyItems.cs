using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.BasketTests;

public class BasketRemoveEmptyItemsTests
{
    [Fact]
    public async Task DoesNotRemovesNonEmptyBasketItems()
    {
        // Arrange
        var basket = new Basket("TestBuyerId");
        basket.AddItem(1, 10, 1);

        // Act
        basket.RemoveEmptyItems();

        // Assert
        Assert.Single(basket.Items);
    }
}