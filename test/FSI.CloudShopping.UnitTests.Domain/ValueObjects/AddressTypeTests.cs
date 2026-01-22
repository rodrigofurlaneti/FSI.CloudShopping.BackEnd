using FluentAssertions;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.UnitTests.ValueObjects;

public class AddressTypeTests
{
    [Theory]
    [InlineData("Shipping", "Shipping")]
    [InlineData("Billing", "Billing")]
    [InlineData("Commercial", "Commercial")]
    public void FromString_ShouldReturnCorrectAddressType_WhenValueIsValid(string input, string expectedDescription)
    {
        // Act
        var result = AddressType.FromString(input);

        // Assert
        result.Description.Should().Be(expectedDescription);
    }

    [Fact]
    public void FromString_ShouldThrowArgumentException_WhenValueIsInvalid()
    {
        // Arrange
        var invalidType = "Office";

        // Act
        Action act = () => AddressType.FromString(invalidType);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("Invalid Address Type");
    }

    [Fact]
    public void StaticProperties_ShouldReturnCorrectDescriptions()
    {
        // Assert
        AddressType.Shipping.Description.Should().Be("Shipping");
        AddressType.Billing.Description.Should().Be("Billing");
        AddressType.Commercial.Description.Should().Be("Commercial");
    }

    [Fact]
    public void AddressType_ShouldSupportValueEquality()
    {
        // Arrange
        var type1 = AddressType.FromString("Shipping");
        var type2 = AddressType.Shipping;

        // Assert
        // Como é um record, a igualdade por valor é nativa
        type1.Should().Be(type2);
        (type1 == type2).Should().BeTrue();
    }
}