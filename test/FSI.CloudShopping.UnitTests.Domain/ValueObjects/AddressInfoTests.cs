using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class AddressInfoTests
    {
        [Fact]
        public void Constructor_ShouldCreateAddressInfo_WhenAllParametersAreValid()
        {
            // Arrange
            var street = "Avenida Paulista";
            var number = "1000";
            var zipCode = "01310-100";
            var city = "São Paulo";
            var state = "SP";

            // Act
            var address = new AddressInfo(street, number, zipCode, city, state);

            // Assert
            address.Street.Should().Be(street);
            address.Number.Should().Be(number);
            address.ZipCode.Should().Be(zipCode);
            address.City.Should().Be(city);
            address.State.Should().Be(state);
        }

        [Theory]
        [InlineData("", "1", "01000-000", "City", "ST")] // Street vazia
        [InlineData("Street", "", "01000-000", "City", "ST")] // Number vazio
        [InlineData("Street", "1", "", "City", "ST")] // ZipCode vazio
        public void Constructor_ShouldThrowDomainException_WhenParametersAreInvalid(
            string street, string number, string zipCode, string city, string state)
        {
            // Act
            Action act = () => new AddressInfo(street, number, zipCode, city, state);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void AddressInfo_ShouldBeImmutable_AndSupportValueEquality()
        {
            // Arrange
            var addr1 = new AddressInfo("Rua A", "10", "01000-000", "SP", "SP");
            var addr2 = new AddressInfo("Rua A", "10", "01000-000", "SP", "SP");

            // Act & Assert
            // Como é um record, a comparação deve ser por valor e não por referência
            addr1.Should().Be(addr2);
            addr1.Equals(addr2).Should().BeTrue();
        }
    }
}
