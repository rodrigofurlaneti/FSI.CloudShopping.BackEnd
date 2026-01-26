using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class CompanyTests
    {
        private const string ValidCnpj = "45723174000110";

        [Fact]
        public void Constructor_Should_Set_Properties_Correctly()
        {
            var taxId = new BusinessTaxId(ValidCnpj);
            var company = new Company(1, taxId, "Cloud Services LTDA", "123456");

            company.BusinessTaxId.Should().Be(taxId);
            company.CompanyName.Should().Be("Cloud Services LTDA");
            company.Id.Should().Be(1);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void UpdateCompanyName_Should_Throw_When_Name_Is_Invalid(string invalidName)
        {
            var company = CriarCompany();

            Action act = () => company.UpdateCompanyName(invalidName);

            act.Should().Throw<DomainException>()
               .WithMessage("O nome da empresa não pode ser vazio.");
        }

        private Company CriarCompany()
        {
            return new Company(1, new BusinessTaxId(ValidCnpj), "Empresa Teste", null);
        }
    }
}