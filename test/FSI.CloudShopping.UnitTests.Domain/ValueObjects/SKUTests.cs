using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class SKUTests
    {
        [Fact]
        public void Should_Create_SKU_When_Code_Is_Valid()
        {
            var sku = new SKU("abc-123");

            sku.Code.Should().Be("ABC-123");
        }

        [Fact]
        public void Should_Trim_And_Uppercase_SKU_Code()
        {
            var sku = new SKU("  prod-001  ");

            sku.Code.Should().Be("PROD-001");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Throw_Exception_When_Code_Is_Invalid(string code)
        {
            var act = () => new SKU(code);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("SKU code cannot be empty.");
        }

        [Fact]
        public void ToString_Should_Return_Code()
        {
            var sku = new SKU("sku-999");

            sku.ToString().Should().Be("SKU-999");
        }
    }
}
