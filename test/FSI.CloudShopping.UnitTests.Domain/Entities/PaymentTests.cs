using System;
using FluentAssertions;
using Xunit;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class PaymentTests
    {
        [Fact]
        public void Constructor_Should_Throw_When_OrderId_Is_Invalid()
        {
            Action act = () =>
                new Payment(0, PaymentMethod.CreditCard, new Money(100));

            act.Should()
                .Throw<DomainException>()
                .WithMessage("O ID do pedido é obrigatório para um pagamento.");
        }

        [Fact]
        public void Constructor_Should_Throw_When_Amount_Is_Zero_Or_Negative()
        {
            Action act = () =>
                new Payment(1, PaymentMethod.Pix, new Money(0));

            act.Should()
                .Throw<DomainException>()
                .WithMessage("O valor do pagamento deve ser maior que zero.");
        }

        [Fact]
        public void Constructor_Should_Create_Payment_With_Pending_Status()
        {
            var payment = new Payment(
                orderId: 10,
                method: PaymentMethod.CreditCard,
                amount: new Money(250)
            );

            payment.OrderId.Should().Be(10);
            payment.Method.Should().Be(PaymentMethod.CreditCard);
            payment.Amount.Value.Should().Be(250);
            payment.Status.Should().Be(PaymentStatus.Pending);
        }

        [Fact]
        public void Capture_Should_Change_Status_To_Captured_When_Pending()
        {
            var payment = new Payment(
                1,
                PaymentMethod.Pix,
                new Money(100)
            );

            payment.Capture();

            payment.Status.Should().Be(PaymentStatus.Captured);
        }

        [Fact]
        public void Capture_Should_Throw_When_Status_Is_Not_Pending()
        {
            var payment = new Payment(
                1,
                PaymentMethod.Invoice,
                new Money(100)
            );

            payment.Capture(); 

            Action act = () => payment.Capture();

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Apenas pagamentos pendentes podem ser capturados.");
        }

        [Fact]
        public void Fail_Should_Set_Status_To_Failed()
        {
            var payment = new Payment(
                1,
                PaymentMethod.CreditCard,
                new Money(100)
            );

            payment.Fail();

            payment.Status.Should().Be(PaymentStatus.Failed);
        }

        [Fact]
        public void Refund_Should_Change_Status_To_Refunded_When_Captured()
        {
            var payment = new Payment(
                1,
                PaymentMethod.CreditCard,
                new Money(100)
            );

            payment.Capture();
            payment.Refund();

            payment.Status.Should().Be(PaymentStatus.Refunded);
        }

        [Fact]
        public void Refund_Should_Throw_When_Status_Is_Not_Captured()
        {
            var payment = new Payment(
                1,
                PaymentMethod.CreditCard,
                new Money(100)
            );

            Action act = () => payment.Refund();

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Apenas pagamentos capturados podem ser reembolsados.");
        }
    }
}
