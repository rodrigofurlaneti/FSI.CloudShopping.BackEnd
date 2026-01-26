using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class BusinessTaxIdTests
    {
        #region Valid CNPJ

        [Fact]
        public void Constructor_Should_Create_BusinessTaxId_When_Cnpj_Is_Valid()
        {
            // Arrange
            var cnpj = "45.723.174/0001-10"; // CNPJ válido

            // Act
            var businessTaxId = new BusinessTaxId(cnpj);

            // Assert
            businessTaxId.Number.Should().Be("45723174000110");
        }

        [Fact]
        public void Constructor_Should_Remove_Mask_From_Cnpj()
        {
            // Arrange
            var masked = "45.723.174/0001-10";

            // Act
            var businessTaxId = new BusinessTaxId(masked);

            // Assert
            businessTaxId.Number.Should().MatchRegex("^[0-9]+$");
            businessTaxId.Number.Should().HaveLength(14);
        }

        #endregion

        #region Empty / Null

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_Cnpj_Is_Empty(string cnpj)
        {
            Action act = () => new BusinessTaxId(cnpj);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("O CNPJ não pode ser vazio.");
        }

        [Fact]
        public void Constructor_Should_Throw_When_Cnpj_Is_Null()
        {
            Action act = () => new BusinessTaxId(null!);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("O CNPJ não pode ser vazio.");
        }

        #endregion

        #region Invalid CNPJ

        [Theory]
        [InlineData("123")] // muito curto
        [InlineData("123456789012345")] // muito longo
        [InlineData("11111111111111")] // números repetidos
        [InlineData("00000000000000")] // números repetidos
        [InlineData("12345678901234")] // dígito verificador inválido
        public void Constructor_Should_Throw_When_Cnpj_Is_Invalid(string cnpj)
        {
            Action act = () => new BusinessTaxId(cnpj);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("Número de CNPJ inválido.");
        }

        #endregion

        #region Equality (Record)

        [Fact]
        public void BusinessTaxId_Should_Be_Equal_When_Number_Is_Same()
        {
            var cnpj1 = new BusinessTaxId("45.723.174/0001-10");
            var cnpj2 = new BusinessTaxId("45723174000110");

            cnpj1.Should().Be(cnpj2);
            cnpj1.GetHashCode().Should().Be(cnpj2.GetHashCode());
        }

        [Fact]
        public void BusinessTaxId_Should_Not_Be_Equal_When_Number_Is_Different()
        {
            var cnpj1 = new BusinessTaxId("45.723.174/0001-10");
            var cnpj2 = new BusinessTaxId("04.252.011/0001-10");

            cnpj1.Should().NotBe(cnpj2);
        }

        #endregion

        #region ToString
        #endregion
    }
}
