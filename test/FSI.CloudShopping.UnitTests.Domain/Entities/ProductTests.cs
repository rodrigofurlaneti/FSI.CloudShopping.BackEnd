using System;
using FluentAssertions;
using Xunit;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class ProductTests
    {
        private static Product CreateProduct(
            int stock = 10,
            decimal price = 100)
        {
            return new Product(
                new SKU("SKU-001"),
                "Notebook Gamer",
                new Money(price),
                new Quantity(stock)
            );
        }

        [Fact]
        public void Constructor_Should_Throw_When_Name_Is_Empty()
        {
            Action act = () => new Product(
                new SKU("SKU-001"),
                "",
                new Money(100),
                new Quantity(10)
            );

            act.Should()
                .Throw<DomainException>()
                .WithMessage("O nome do produto é obrigatório.");
        }

        [Fact]
        public void Constructor_Should_Set_All_Properties()
        {
            var sku = new SKU("SKU-123");
            var price = new Money(150);
            var stock = new Quantity(20);

            var product = new Product(sku, "Mouse Gamer", price, stock);

            product.Sku.Should().Be(sku);
            product.Name.Should().Be("Mouse Gamer");
            product.Price.Should().Be(price);
            product.Stock.Should().Be(stock);
        }

        [Fact]
        public void UpdateStock_Should_Update_When_Quantity_Is_Valid()
        {
            var product = CreateProduct();

            product.UpdateStock(new Quantity(50));

            product.Stock.Value.Should().Be(50);
        }

        [Fact]
        public void UpdateStock_Should_Throw_When_Quantity_Is_Invalid()
        {
            var product = CreateProduct();
            Action act = () => product.UpdateStock(new Quantity(-1));
            act.Should().Throw<DomainException>()
               .WithMessage("Quantity must be at least 1.");
        }

        [Fact]
        public void DebitStock_Should_Throw_When_Stock_Is_Insufficient_Version()
        {
            var product = CreateProduct(stock: 2);
            Action act = () => product.DebitStock(new Quantity(5));
            act.Should()
                .Throw<DomainException>()
                .WithMessage("*Estoque insuficiente*Notebook Gamer*");
        }

        [Fact]
        public void UpdatePrice_Should_Change_Product_Price()
        {
            var product = CreateProduct();

            product.UpdatePrice(new Money(250));

            product.Price.Value.Should().Be(250);
        }

        [Fact]
        public void DebitStock_Should_Decrease_Stock_When_Enough_Quantity()
        {
            var product = CreateProduct(stock: 10);

            product.DebitStock(new Quantity(4));

            product.Stock.Value.Should().Be(6);
        }

        [Fact]
        public void DebitStock_Should_Throw_When_Stock_Is_Insufficient()
        {
            var product = CreateProduct(stock: 2);

            Action act = () => product.DebitStock(new Quantity(5));

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Estoque insuficiente para o produto Notebook Gamer.");
        }

        [Fact]
        public void CreditStock_Should_Increase_Stock()
        {
            var product = CreateProduct(stock: 5);

            product.CreditStock(new Quantity(3));

            product.Stock.Value.Should().Be(8);
        }
    }
}
