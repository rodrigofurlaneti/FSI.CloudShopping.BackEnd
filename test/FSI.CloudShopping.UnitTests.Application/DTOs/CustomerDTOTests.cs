using FSI.CloudShopping.Application.DTOs;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.UnitTests.Application.DTOs
{
    public class CustomerDTOTests
    {
        [Fact]
        [Trait("Category", "DTO")]
        public void Init_ShouldSetAllPropertiesCorrectly()
        {
            // Arrange
            var expectedId = 123;
            var expectedName = "João Silva";
            var expectedEmail = "joao@email.com";
            var expectedTaxId = "123.456.789-00";
            var expectedPhone = "11999999999";
            var expectedPassword = "SecurePassword123";
            var expectedIsCompany = false;

            // Act
            var dto = new CustomerDTO
            {
                Id = expectedId,
                Name = expectedName,
                Email = expectedEmail,
                TaxId = expectedTaxId,
                Phone = expectedPhone,
                Password = expectedPassword,
                IsCompany = expectedIsCompany
            };

            // Assert
            dto.Id.Should().Be(expectedId);
            dto.Name.Should().Be(expectedName);
            dto.Email.Should().Be(expectedEmail);
            dto.TaxId.Should().Be(expectedTaxId);
            dto.Phone.Should().Be(expectedPhone);
            dto.Password.Should().Be(expectedPassword);
            dto.IsCompany.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Records_WithSameValues_ShouldBeEqual()
        {
            // Arrange
            var dto1 = new CustomerDTO { Id = 1, Email = "test@test.com" };
            var dto2 = new CustomerDTO { Id = 1, Email = "test@test.com" };

            // Assert
            dto1.Should().Be(dto2);
            (dto1 == dto2).Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Records_WithDifferentTaxId_ShouldNotBeEqual()
        {
            // Arrange
            var dto1 = new CustomerDTO { Id = 1, TaxId = "111" };
            var dto2 = new CustomerDTO { Id = 1, TaxId = "222" };

            // Assert
            dto1.Should().NotBe(dto2);
        }
    }
}