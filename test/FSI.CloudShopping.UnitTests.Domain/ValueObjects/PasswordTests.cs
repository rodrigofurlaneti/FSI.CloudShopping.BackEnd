using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class PasswordTests
    {
        [Fact]
        public void Constructor_ShouldCreatePassword_WhenHashIsValid()
        {
            // Arrange
            var validHash = "AQAAAAIAAYagAAAAEOf6k6mXF3V8..."; // Simulando um hash real
            // Act
            var password = new Password(validHash);
            // Assert
            password.Hash.Should().Be(validHash);
        }
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_ShouldThrowDomainException_WhenHashIsEmpty(string? invalidHash)
        {
            // Act
            Action act = () => new Password(invalidHash!);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Password hash is required.");
        }
        [Fact]
        public void Constructor_ShouldThrowDomainException_WhenHashIsTooShort()
        {
            // Act
            Action act = () => new Password("12345");
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Invalid password hash format.");
        }
        [Fact]
        public void ToString_ShouldNotRevealHash()
        {
            // Arrange
            var password = new Password("AQAAAAIAAYagAAAAEOf6k6mXF3V8...");
            // Assert
            password.ToString().Should().Be("********");
        }
    }
}
