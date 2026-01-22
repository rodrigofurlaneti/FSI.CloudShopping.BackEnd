using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Services;
using Moq;
using Xunit;

namespace FSI.CloudShopping.UnitTests.Domain.Services
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _cartRepoMock = new Mock<ICartRepository>();
            _cartService = new CartService(_cartRepoMock.Object);
        }

        [Fact]
        public async Task MergeCart_Should_Transfer_Items_From_Visitor_To_Customer()
        {
            // Arrange
            var token = new VisitorToken(Guid.NewGuid());
            var customerId = 123;

            var visitorCart = new Cart(token);
            visitorCart.AddOrUpdateItem(1, new Quantity(2), new Money(50));

            var customerCart = new Cart(customerId);

            _cartRepoMock.Setup(r => r.GetByVisitorTokenAsync(token)).ReturnsAsync(visitorCart);
            _cartRepoMock.Setup(r => r.GetByCustomerIdAsync(customerId)).ReturnsAsync(customerCart);

            // Act
            await _cartService.MergeCartAsync(token, customerId);

            // Assert
            Assert.Single(customerCart.Items);
            _cartRepoMock.Verify(r => r.RemoveAsync(visitorCart.Id), Times.Once);
            _cartRepoMock.Verify(r => r.UpdateAsync(customerCart), Times.Once);
        }

        [Fact]
        public async Task MergeCartAsync_ShouldTransferItemsAndRemoveVisitorCart()
        {
            // Arrange
            var token = new VisitorToken(Guid.NewGuid());
            var customerId = 99;
            var visitorCart = new Cart(token);
            visitorCart.AddOrUpdateItem(1, new Quantity(2), new Money(10));

            _cartRepoMock.Setup(r => r.GetByVisitorTokenAsync(token)).ReturnsAsync(visitorCart);
            _cartRepoMock.Setup(r => r.GetByCustomerIdAsync(customerId)).ReturnsAsync((Cart?)null);

            // Act
            await _cartService.MergeCartAsync(token, customerId);

            // Assert
            _cartRepoMock.Verify(r => r.UpdateAsync(It.Is<Cart>(c => c.CustomerId == customerId)), Times.Once);
            _cartRepoMock.Verify(r => r.RemoveAsync(visitorCart.Id), Times.Once);
        }
    }
}
