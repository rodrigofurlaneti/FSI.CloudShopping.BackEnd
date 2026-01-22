using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class VisitorTokenTests
    {
        [Fact]
        public void Constructor_ShouldCreateToken_WhenGuidIsValid()
        {
            // Arrange
            var validGuid = Guid.NewGuid();
            // Act
            var token = new VisitorToken(validGuid);
            // Assert
            token.Value.Should().Be(validGuid);
        }
        [Fact]
        public void Constructor_ShouldThrowDomainException_WhenGuidIsEmpty()
        {
            // Act
            Action act = () => new VisitorToken(Guid.Empty);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Visitor token cannot be empty.");
        }
        [Fact]
        public void NewToken_ShouldGenerateUniqueTokens()
        {
            // Act
            var token1 = VisitorToken.NewToken();
            var token2 = VisitorToken.NewToken();
            // Assert
            token1.Should().NotBe(token2);
            token1.Value.Should().NotBe(Guid.Empty);
        }
        [Fact]
        public void VisitorToken_ShouldSupportValueEquality()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var token1 = new VisitorToken(guid);
            var token2 = new VisitorToken(guid);
            // Assert
            token1.Should().Be(token2);
        }
    }
}
