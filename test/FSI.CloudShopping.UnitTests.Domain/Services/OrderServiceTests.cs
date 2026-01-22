using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
using FSI.CloudShopping.Domain.Services;
using FSI.CloudShopping.Domain.ValueObjects;
using Moq;
using Xunit;

namespace FSI.CloudShopping.UnitTests.Domain.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<ICartRepository> _cartRepo;
        private readonly Mock<IOrderRepository> _orderRepo;
        private readonly Mock<IInventoryService> _inventory;
        private readonly OrderService _orderService;
        public OrderServiceTests()
        {
            _cartRepo = new Mock<ICartRepository>();
            _orderRepo = new Mock<IOrderRepository>();
            _inventory = new Mock<IInventoryService>();
            _orderService = new OrderService(_cartRepo.Object, _orderRepo.Object, _inventory.Object);
        }
        [Fact]
        public async Task PlaceOrder_ShouldFail_WhenStockIsInsufficient()
        {
            // Arrange
            var cart = new Cart(1);
            cart.AddOrUpdateItem(1, new Quantity(1), new Money(10));
            _cartRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(cart);
            _inventory.Setup(i => i.ValidateAndReserveStockAsync(It.IsAny<IEnumerable<CartItem>>()))
                      .ThrowsAsync(new DomainException("Estoque insuficiente"));
            // Act
            Func<Task> act = async () => await _orderService.PlaceOrderAsync(1);
            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Estoque insuficiente");
        }
    }
}
