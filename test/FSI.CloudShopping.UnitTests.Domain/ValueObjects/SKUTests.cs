using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class SKUTests
    {
        [Theory]
        [InlineData("iphone-15-pro", "IPHONE-15-PRO")]
        [InlineData("  PROD123  ", "PROD123")]
        [InlineData("Notebook_Dell", "NOTEBOOK_DELL")]
        public void Constructor_ShouldSanitizeAndNormalize_WhenInputIsValid(string input, string expected)
        {
            // Act
            var sku = new SKU(input);
            // Assert
            sku.Code.Should().Be(expected);
        }
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_ShouldThrowDomainException_WhenCodeIsEmpty(string? input)
        {
            // Act
            Action act = () => new SKU(input!);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("SKU code cannot be empty.");
        }

        [Fact]
        public void SKU_ShouldSupportValueEquality()
        {
            // Arrange
            var sku1 = new SKU("prod-abc");
            var sku2 = new SKU("PROD-ABC ");
            // Assert
            // A igualdade deve funcionar mesmo com inputs de cases diferentes devido ao ToUpper()
            sku1.Should().Be(sku2);
            (sku1 == sku2).Should().BeTrue();
        }
        [Fact]
        public void ToString_ShouldReturnTheCode()
        {
            // Arrange
            var sku = new SKU("test-123");
            // Assert
            sku.ToString().Should().Be("TEST-123");
        }
    }
}
