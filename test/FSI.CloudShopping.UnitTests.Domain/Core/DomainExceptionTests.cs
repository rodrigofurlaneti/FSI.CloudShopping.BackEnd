using System;
using FSI.CloudShopping.Domain.Core;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.Core
{
    public class DomainExceptionTests
    {
        [Fact]
        public void Constructor_DeveCriarUmaExceptionDoTipoException()
        {
            // Arrange
            var message = "Erro de domínio";
            // Act
            var exception = new DomainException(message);
            // Assert
            exception.Should().BeAssignableTo<Exception>();
        }

        [Fact]
        public void Constructor_DeveAtribuirAMensagemCorretamente()
        {
            // Arrange
            var message = "Mensagem de erro de domínio";
            // Act
            var exception = new DomainException(message);
            // Assert
            exception.Message.Should().Be(message);
        }

        [Fact]
        public void Constructor_NaoDeveSerNulo()
        {
            // Arrange & Act
            var exception = new DomainException("Erro");
            // Assert
            exception.Should().NotBeNull();
        }
    }
}
