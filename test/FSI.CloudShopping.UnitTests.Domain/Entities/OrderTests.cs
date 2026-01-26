using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class OrderTests
    {
        private static CartItem CreateCartItem(
            int productId,
            int quantity,
            decimal unitPrice)
        {
            return new CartItem(
                productId,
                new Quantity(quantity),
                new Money(unitPrice)
            );
        }

        [Fact]
        public void Constructor_Should_Throw_When_CustomerId_Is_Invalid()
        {
            var cartItems = new List<CartItem>
            {
                CreateCartItem(1, 2, 10)
            };

            Action act = () => new Order(0, 1, cartItems);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Cliente inválido.");
        }

        [Fact]
        public void Constructor_Should_Throw_When_ShippingAddressId_Is_Invalid()
        {
            var cartItems = new List<CartItem>
            {
                CreateCartItem(1, 2, 10)
            };

            Action act = () => new Order(1, 0, cartItems);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Endereço de entrega é obrigatório.");
        }

        [Fact]
        public void Constructor_Should_Throw_When_Cart_Is_Empty()
        {
            Action act = () => new Order(1, 1, new List<CartItem>());

            act.Should()
                .Throw<DomainException>()
                .WithMessage("O pedido deve ter pelo menos um item.");
        }

        [Fact]
        public void Constructor_Should_Create_Order_With_Valid_Data()
        {
            var cartItems = new List<CartItem>
            {
                CreateCartItem(1, 2, 10),
                CreateCartItem(2, 1, 20)
            };

            var order = new Order(
                customerId: 10,
                shippingAddressId: 5,
                cartItems: cartItems
            );

            order.CustomerId.Should().Be(10);
            order.ShippingAddressId.Should().Be(5);
            order.Status.Should().Be(OrderStatus.Pending);
            order.Items.Should().HaveCount(2);
            order.TotalAmount.Value.Should().Be(40); // (2 * 10) + (1 * 20)
            order.OrderDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void Constructor_Should_Convert_CartItems_To_OrderItems()
        {
            var cartItems = new List<CartItem>
            {
                CreateCartItem(1, 3, 15)
            };

            var order = new Order(1, 1, cartItems);
            var item = order.Items.Should().ContainSingle().Subject;

            item.ProductId.Should().Be(1);
            item.Quantity.Value.Should().Be(3);
            item.UnitPrice.Value.Should().Be(15);
        }

        [Fact]
        public void ChangeStatus_Should_Update_Order_Status()
        {
            var cartItems = new List<CartItem>
            {
                CreateCartItem(1, 1, 50)
            };

            var order = new Order(1, 1, cartItems);

            order.ChangeStatus(OrderStatus.Paid);

            order.Status.Should().Be(OrderStatus.Paid);
        }
    }
}
