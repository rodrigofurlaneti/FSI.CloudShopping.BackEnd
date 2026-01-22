using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class TaxIdTests
    {
        [Theory]
        [InlineData("123.456.789-01", false)]
        [InlineData("12.345.678/0001-99", true)]
        public void Should_Identify_Correct_TaxId_Type(string input, bool expectedIsCompany)
        {
            var taxId = new TaxId(input);
            Assert.Equal(expectedIsCompany, taxId.IsCompany);
        }

        [Fact]
        public void Should_Throw_Exception_For_Invalid_Length()
        {
            Assert.Throws<ArgumentException>(() => new TaxId("123"));
        }
    }
}
