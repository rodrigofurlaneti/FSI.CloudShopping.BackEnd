using FSI.CloudShopping.Application.DTOs;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.UnitTests.Application.DTOs
{
    public class AddressDTOTests
    {
        [Fact]
        [Trait("Category", "DTO")]
        public void Constructor_ShouldSetAllPropertiesCorrectly()
        {
            // Arrange
            var street = "Avenida Paulista";
            var number = "1000";
            var neighborhood = "Bela Vista";
            var city = "São Paulo";
            var state = "SP";
            var zipCode = "01310-100";
            var isDefault = true;

            // Act
            var dto = new AddressDTO(street, number, neighborhood, city, state, zipCode, isDefault);

            // Assert
            dto.Street.Should().Be(street);
            dto.Number.Should().Be(number);
            dto.Neighborhood.Should().Be(neighborhood);
            dto.City.Should().Be(city);
            dto.State.Should().Be(state);
            dto.ZipCode.Should().Be(zipCode);
            dto.IsDefault.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Records_WithIdenticalAddresses_ShouldBeEqual()
        {
            // Arrange
            var dto1 = new AddressDTO("Rua A", "10", "Centro", "Rio", "RJ", "20000-000", false);
            var dto2 = new AddressDTO("Rua A", "10", "Centro", "Rio", "RJ", "20000-000", false);

            // Assert
            dto1.Should().Be(dto2);
            (dto1 == dto2).Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Records_WithDifferentDefaultStatus_ShouldNotBeEqual()
        {
            // Arrange
            var dto1 = new AddressDTO("Rua A", "10", "Centro", "Rio", "RJ", "20000-000", true);
            var dto2 = new AddressDTO("Rua A", "10", "Centro", "Rio", "RJ", "20000-000", false);

            // Assert
            dto1.Should().NotBe(dto2);
        }
    }
}