using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class AddressTests
    {
        [Fact]
        public void Constructor_ShouldCreateAddress_WhenAllParametersAreValid()
        {
            // Arrange & Act
            var address = new Address("Av. Paulista", "1000", "Bela Vista", "01310-100", "SP", "SP", true);

            // Assert
            address.Street.Should().Be("Av. Paulista");
            address.IsDefault.Should().BeTrue();
        }

        [Fact]
        public void Address_ShouldSupportValueEquality()
        {
            // Arrange
            var addr1 = new Address("Rua A", "10", "Centro", "01000-000", "SP", "SP", true);
            var addr2 = new Address("Rua A", "10", "Centro", "01000-000", "SP", "SP", true);

            // Assert
            addr1.Should().Be(addr2);
        }
    }
}
