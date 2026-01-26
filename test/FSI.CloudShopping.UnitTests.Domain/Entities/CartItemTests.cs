using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class CartItemTests
    {
        [Fact]
        public void Constructor_DeveCriarItemComValoresInformados()
        {
            // Arrange
            var productId = 10;
            var quantity = new Quantity(2);
            var unitPrice = new Money(15);

            // Act
            var item = new CartItem(productId, quantity, unitPrice);

            // Assert
            item.ProductId.Should().Be(productId);
            item.Quantity.Should().Be(quantity);
            item.UnitPrice.Should().Be(unitPrice);
        }

        [Fact]
        public void Constructor_DeveLancarDomainException_QuandoProductIdInvalido()
        {
            // Arrange
            var quantity = new Quantity(1);
            var unitPrice = new Money(10);

            // Act
            var act = () => new CartItem(0, quantity, unitPrice);

            // Assert
            act.Should()
               .Throw<DomainException>()
               .WithMessage("Produto inválido.");
        }

        [Fact]
        public void UpdateQuantity_DeveAtualizarQuantidade()
        {
            // Arrange
            var item = new CartItem(1, new Quantity(1), new Money(10));
            var newQuantity = new Quantity(5);

            // Act
            item.UpdateQuantity(newQuantity);

            // Assert
            item.Quantity.Should().Be(newQuantity);
            item.Quantity.Value.Should().Be(5);
        }

        [Fact]
        public void TotalPrice_DeveRetornarValorTotalCorreto()
        {
            // Arrange
            var item = new CartItem(1, new Quantity(3), new Money(20));

            // Act
            var total = item.TotalPrice;

            // Assert
            total.Value.Should().Be(60);
        }

        [Fact]
        public void CartItem_DeveHerdarDeEntity()
        {
            // Arrange & Act
            var item = new CartItem(1, new Quantity(1), new Money(10));

            // Assert
            item.Should().BeAssignableTo<Entity>();
        }
    }
}
