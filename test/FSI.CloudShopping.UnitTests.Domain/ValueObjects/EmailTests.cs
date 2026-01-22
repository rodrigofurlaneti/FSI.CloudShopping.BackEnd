using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;
namespace FSI.CloudShopping.Domain.UnitTests.ValueObjects
{
    public class EmailTests
    {
        [Theory]
        [InlineData("rodrigo@furlaneti.com")]
        [InlineData("contato@ti.com.br")]
        public void Constructor_ShouldCreateEmail_WhenFormatIsValid(string address)
        {
            // Act
            var email = new Email(address);

            // Assert
            email.Address.Should().Be(address);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("rodrigo.furlaneti.com")] // Sem @
        [InlineData(null)]
        public void Constructor_ShouldThrowDomainException_WhenFormatIsInvalid(string? invalidAddress)
        {
            // Act
            Action act = () => new Email(invalidAddress!);

            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Invalid email address format.");
        }
        [Fact]
        public void ImplicitOperator_ShouldConvertToStringCorrecty()
        {
            // Arrange
            var emailAddress = "test@domain.com";
            var email = new Email(emailAddress);

            // Act
            string result = email;

            // Assert
            result.Should().Be(emailAddress);
        }
    }
}