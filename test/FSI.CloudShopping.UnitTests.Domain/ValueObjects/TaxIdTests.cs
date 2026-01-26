using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class TaxIdTests
    {
        [Theory]
        [InlineData("11111111111")] // Números repetidos
        [InlineData("12345678901")] // Algoritmo inválido
        [InlineData("12345")]       // Muito curto
        [InlineData("123456789012")] // Muito longo
        public void Constructor_Should_Throw_Exception_When_Cpf_Is_Invalid(string invalidCpf)
        {
            // Act
            Action act = () => new TaxId(invalidCpf);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Número de CPF inválido.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_Exception_When_Cpf_Is_Empty(string emptyCpf)
        {
            // Act
            Action act = () => new TaxId(emptyCpf!);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("O CPF não pode ser vazio.");
        }
    }
}