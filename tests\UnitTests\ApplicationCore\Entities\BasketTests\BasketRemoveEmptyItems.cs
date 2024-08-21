using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.BasketTests;

public class BasketRemoveEmptyItems
{
    [Fact]
    public async Task DoesNotRemovesNonEmptyBasketItems()
    {
        var basket = new Basket("buyerId");
        basket.AddItem(1, 10, 1); // Add item with quantity greater than 0
        basket.RemoveEmptyItems();

        Assert.Single(basket.Items); // Assert that item is still in the basket
    }
}