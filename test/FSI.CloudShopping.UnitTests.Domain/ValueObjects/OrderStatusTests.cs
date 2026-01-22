using FluentAssertions;
using FSI.CloudShopping.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class OrderStatusTests
    {
        [Theory]
        [InlineData("Pending")]
        [InlineData("Paid")]
        [InlineData("Shipped")]
        [InlineData("Delivered")]
        [InlineData("Cancelled")]
        public void OrderStatus_ShouldHaveCorrectCode(string expectedCode)
        {
            // Act & Assert
            // Verificamos se cada propriedade estática retorna o código esperado
            var status = expectedCode switch
            {
                "Pending" => OrderStatus.Pending,
                "Paid" => OrderStatus.Paid,
                "Shipped" => OrderStatus.Shipped,
                "Delivered" => OrderStatus.Delivered,
                "Cancelled" => OrderStatus.Cancelled,
                _ => throw new ArgumentException("Invalid status code")
            };

            status.Code.Should().Be(expectedCode);
        }

        [Fact]
        public void OrderStatus_ShouldBeImmutable_AndSupportValueEquality()
        {
            // Arrange
            var status1 = OrderStatus.Pending;
            var status2 = OrderStatus.Pending;
            var status3 = OrderStatus.Paid;

            // Assert
            // Records garantem igualdade por valor
            status1.Should().Be(status2);
            status1.Should().NotBe(status3);
            (status1 == status2).Should().BeTrue();
        }

        [Fact]
        public void OrderStatus_ShouldNotAllowExternalInstantiation()
        {
            // Como o construtor é privado, este teste é conceitual. 
            // O compilador já impede o 'new OrderStatus("...")'.
            // Isso garante que apenas os status mapeados pelo domínio existam.
            var constructor = typeof(OrderStatus).GetConstructors(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            constructor.Should().BeEmpty();
        }
    }
}
