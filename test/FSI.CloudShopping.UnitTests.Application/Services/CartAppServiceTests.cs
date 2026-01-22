using FSI.CloudShopping.Application.Services;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
using FSI.CloudShopping.Domain.ValueObjects;
using Moq;
namespace FSI.CloudShopping.UnitTests.Application.Services
{
    public class CartAppServiceTests
    {
        [Fact]
        public async Task AddItem_ShouldCallRepository_WhenProductIsAvailable()
        {
            // Arrange
            var cartRepoMock = new Mock<ICartRepository>();
            var prodRepoMock = new Mock<IProductRepository>();
            var domainServiceMock = new Mock<ICartService>();
            var product = new Product(new SKU("TEST"), "Product", new Money(10), new Quantity(100));
            prodRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            var service = new CartAppService(cartRepoMock.Object, prodRepoMock.Object, domainServiceMock.Object);
            var token = Guid.NewGuid().ToString();
            // Act
            await service.AddItemAsync(token, new AddItemDTO(1, 2));
            // Assert
            cartRepoMock.Verify(r => r.AddAsync(It.IsAny<Cart>()), Times.Once);
        }
    }
}
