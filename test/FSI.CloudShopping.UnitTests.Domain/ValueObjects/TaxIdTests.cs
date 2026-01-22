using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.UnitTests.ValueObjects
{
    public class TaxIdValidationTests
    {
        [Theory]
        [Trait("Category", "B2B")]
        [InlineData("11.222.333/0001-81")]   // CNPJ Válido com Máscara
        public void Constructor_ShouldAccept_ValidCNPJ(string validCnpj)
        {
            // Act
            var taxId = new TaxId(validCnpj);

            // Assert
            taxId.IsCompany.Should().BeTrue();
            taxId.Number.Should().HaveLength(14);
        }

        [Theory]
        [Trait("Category", "Validation")]
        [InlineData("111.111.111-11")]       // CPF repetido (Falso Positivo Comum)
        [InlineData("22222222222222")]       // CNPJ repetido
        [InlineData("123.456.789-01")]       // CPF com dígitos errados
        [InlineData("12.345.678/0001-90")]   // CNPJ com dígitos errados
        public void Constructor_ShouldThrow_WhenDocumentIsAlgorithmicallyInvalid(string invalidDocument)
        {
            // Act
            Action act = () => new TaxId(invalidDocument);

            // Assert
            // O '*' no WithMessage permite validar tanto "Invalid CPF" quanto "Invalid CNPJ"
            act.Should().Throw<DomainException>()
               .WithMessage("Invalid * number.");
        }

        [Theory]
        [Trait("Category", "Validation")]
        [InlineData("12345")]                // Curto demais
        [InlineData("123456789012345")]      // Longo demais
        public void Constructor_ShouldThrow_WhenLengthIsInvalid(string invalidLength)
        {
            // Act
            Action act = () => new TaxId(invalidLength);

            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Invalid * number.");
        }

        [Fact]
        [Trait("Category", "Validation")]
        public void Constructor_ShouldThrow_WhenTaxIdIsEmpty()
        {
            // Act
            Action act = () => new TaxId("");

            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("TaxId number cannot be empty.");
        }
    }
}