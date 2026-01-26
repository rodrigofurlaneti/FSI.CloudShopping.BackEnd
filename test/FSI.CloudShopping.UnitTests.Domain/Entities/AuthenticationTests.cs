using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class AuthenticationTests
    {
        [Fact]
        public void Constructor_ComParametros_DeveCriarAuthenticationComValoresCorretos()
        {
            // Arrange
            var id = 1;
            var email = "user@email.com";
            var authorizedAccess = true;
            var createdAt = DateTime.Now;

            // Act
            var authentication = new Authentication(id, email, authorizedAccess, createdAt);

            // Assert
            authentication.Id.Should().Be(id);
            authentication.Email.Should().Be(email);
            authentication.AuthorizedAccess.Should().BeTrue();
        }

        [Fact]
        public void Constructor_Vazio_DeveCriarInstanciaValida()
        {
            // Act
            var authentication = new Authentication();

            // Assert
            authentication.Should().NotBeNull();
        }

        [Fact]
        public void Authentication_DeveHerdarDeEntity()
        {
            // Arrange & Act
            var authentication = new Authentication();

            // Assert
            authentication.Should().BeAssignableTo<Entity>();
        }

        [Fact]
        public void DevePermitirAlterarEmail()
        {
            // Arrange
            var authentication = new Authentication();
            var email = "novo@email.com";

            // Act
            authentication.Email = email;

            // Assert
            authentication.Email.Should().Be(email);
        }

        [Fact]
        public void DevePermitirAlterarAuthorizedAccess()
        {
            // Arrange
            var authentication = new Authentication();

            // Act
            authentication.AuthorizedAccess = true;

            // Assert
            authentication.AuthorizedAccess.Should().BeTrue();
        }
    }
}
