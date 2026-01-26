using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class CartTests
    {
        [Fact]
        public void Constructor_DeveCriarCarrinhoComClienteValido()
        {
            // Act
            var cart = new Cart(customerId: 1);

            // Assert
            cart.CustomerId.Should().Be(1);
            cart.Items.Should().BeEmpty();
            cart.UpdatedAt.Should().BeCloseTo(DateTime.Now, precision: TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Constructor_DeveLancarDomainException_QuandoClienteInvalido()
        {
            // Act
            var act = () => new Cart(customerId: 0);

            // Assert
            act.Should()
               .Throw<DomainException>()
               .WithMessage("O carrinho deve pertencer a um cliente válido.");
        }

        [Fact]
        public void AddOrUpdateItem_DeveAdicionarNovoItem_QuandoNaoExistir()
        {
            // Arrange
            var cart = new Cart(1);
            var quantity = new Quantity(2);
            var price = new Money(10);

            // Act
            cart.AddOrUpdateItem(productId: 100, quantity, price);

            // Assert
            cart.Items.Should().HaveCount(1);
            cart.Items.First().ProductId.Should().Be(100);
            cart.Items.First().Quantity.Value.Should().Be(2);
            cart.Items.First().UnitPrice.Value.Should().Be(10);
        }

        [Fact]
        public void AddOrUpdateItem_DeveSomarQuantidade_QuandoItemJaExistir()
        {
            // Arrange
            var cart = new Cart(1);
            cart.AddOrUpdateItem(100, new Quantity(1), new Money(10));

            // Act
            cart.AddOrUpdateItem(100, new Quantity(2), new Money(10));

            // Assert
            cart.Items.Should().HaveCount(1);
            cart.Items.First().Quantity.Value.Should().Be(3);
        }

        [Fact]
        public void AddOrUpdateItem_DeveAtualizarUpdatedAt()
        {
            // Arrange
            var cart = new Cart(1);
            var before = cart.UpdatedAt;

            // Act
            cart.AddOrUpdateItem(1, new Quantity(1), new Money(5));

            // Assert
            cart.UpdatedAt.Should().BeAfter(before);
        }

        [Fact]
        public void RemoveItem_DeveRemoverItemExistente()
        {
            // Arrange
            var cart = new Cart(1);
            cart.AddOrUpdateItem(1, new Quantity(1), new Money(10));

            // Act
            cart.RemoveItem(1);

            // Assert
            cart.Items.Should().BeEmpty();
        }

        [Fact]
        public void RemoveItem_DeveAtualizarUpdatedAt_QuandoItemExistir()
        {
            // Arrange
            var cart = new Cart(1);
            cart.AddOrUpdateItem(1, new Quantity(1), new Money(10));
            var before = cart.UpdatedAt;

            // Act
            cart.RemoveItem(1);

            // Assert
            cart.UpdatedAt.Should().BeAfter(before);
        }

        [Fact]
        public void RemoveItem_NaoDeveFazerNada_QuandoItemNaoExistir()
        {
            // Arrange
            var cart = new Cart(1);
            var before = cart.UpdatedAt;

            // Act
            cart.RemoveItem(999);

            // Assert
            cart.Items.Should().BeEmpty();
            cart.UpdatedAt.Should().Be(before);
        }

        [Fact]
        public void IsExpired_DeveRetornarTrue_QuandoAtualizadoHaMaisDe30Dias()
        {
            // Arrange
            var cart = new Cart(1);

            // força UpdatedAt via reflexão (teste de domínio)
            typeof(Cart)
                .GetProperty("UpdatedAt")!
                .SetValue(cart, DateTime.Now.AddDays(-31));

            // Act
            var expired = cart.IsExpired();

            // Assert
            expired.Should().BeTrue();
        }

        [Fact]
        public void IsExpired_DeveRetornarFalse_QuandoAtualizadoRecentemente()
        {
            // Arrange
            var cart = new Cart(1);

            // Act
            var expired = cart.IsExpired();

            // Assert
            expired.Should().BeFalse();
        }

        [Fact]
        public void GetTotal_DeveRetornarValorTotalDoCarrinho()
        {
            // Arrange
            var cart = new Cart(1);
            cart.AddOrUpdateItem(1, new Quantity(2), new Money(10)); // 20
            cart.AddOrUpdateItem(2, new Quantity(1), new Money(5));  // 5

            // Act
            var total = cart.GetTotal();

            // Assert
            total.Value.Should().Be(25);
        }
    }
}
