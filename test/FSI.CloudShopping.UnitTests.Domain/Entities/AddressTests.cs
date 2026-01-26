using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class AddressTests
    {
        [Fact]
        public void Constructor_Should_Create_Address_With_Valid_Values()
        {
            var address = new Address(
                customerId: 1,
                addressType: AddressType.Shipping,
                street: "Rua das Flores",
                number: "123",
                city: "São Paulo",
                state: "SP",
                zipCode: "01234-000",
                isDefault: true);

            address.CustomerId.Should().Be(1);
            address.AddressType.Should().Be(AddressType.Shipping);
            address.Street.Should().Be("Rua das Flores");
            address.Number.Should().Be("123");
            address.City.Should().Be("São Paulo");
            address.State.Should().Be("SP");
            address.ZipCode.Should().Be("01234-000");
            address.IsDefault.Should().BeTrue();
        }

        [Fact]
        public void Constructor_Should_Create_Address_As_Non_Default_By_Default()
        {
            var address = new Address(
                customerId: 1,
                addressType: AddressType.Billing,
                street: "Av. Brasil",
                number: "1000",
                city: "Rio de Janeiro",
                state: "RJ",
                zipCode: "20000-000");

            address.IsDefault.Should().BeFalse();
        }

        [Fact]
        public void SetAsDefault_Should_Mark_Address_As_Default()
        {
            var address = CriarEndereco(false);

            address.SetAsDefault();

            address.IsDefault.Should().BeTrue();
        }

        [Fact]
        public void SetNonDefault_Should_Mark_Address_As_Not_Default()
        {
            var address = CriarEndereco(true);

            address.SetNonDefault();

            address.IsDefault.Should().BeFalse();
        }

        [Fact]
        public void Address_Should_Inherit_From_Entity()
        {
            var address = CriarEndereco();

            address.Should().BeAssignableTo<Entity>();
        }

        [Fact]
        public void Constructor_Should_Throw_When_AddressType_Is_Null()
        {
            Action act = () => new Address(
                customerId: 1,
                addressType: null!,
                street: "Rua A",
                number: "10",
                city: "Campinas",
                state: "SP",
                zipCode: "13000-000");

            act.Should()
               .Throw<DomainException>()
               .WithMessage("AddressType é obrigatório.");
        }

        private static Address CriarEndereco(bool isDefault = false)
        {
            return new Address(
                customerId: 1,
                addressType: AddressType.Shipping,
                street: "Rua A",
                number: "10",
                city: "Campinas",
                state: "SP",
                zipCode: "13000-000",
                isDefault: isDefault);
        }
    }
}
