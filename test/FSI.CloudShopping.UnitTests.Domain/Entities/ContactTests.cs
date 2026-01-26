using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class ContactTests
    {
        [Fact]
        public void Constructor_DeveCriarContatoComValoresInformados()
        {
            // Arrange
            var customerId = 1;
            var name = new PersonName("João Silva");
            var email = new Email("joao.silva@email.com");
            var phone = new Phone("11999999999");
            var position = "Gerente";

            // Act
            var contact = new Contact(
                customerId,
                name,
                email,
                phone,
                position);

            // Assert
            contact.CustomerId.Should().Be(customerId);
            contact.Name.Should().Be(name);
            contact.Email.Should().Be(email);
            contact.Phone.Should().Be(phone);
            contact.Position.Should().Be(position);
        }

        [Fact]
        public void Contact_DeveHerdarDeEntity()
        {
            // Arrange
            var contact = CriarContatoPadrao();

            // Act & Assert
            contact.Should().BeAssignableTo<Entity>();
        }

        private static Contact CriarContatoPadrao()
        {
            return new Contact(
                customerId: 1,
                name: new PersonName("Maria Oliveira"),
                email: new Email("maria.oliveira@email.com"),
                phone: new Phone("11988887777"),
                position: "Compras");
        }
    }
}
