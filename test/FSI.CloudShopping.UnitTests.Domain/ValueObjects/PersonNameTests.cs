using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class PersonNameTests
    {
        [Theory]
        [InlineData("Rodrigo Furlaneti")]
        [InlineData("Augusto da Silva")]
        [InlineData("Jose Maria de Souza")]
        public void Constructor_ShouldCreatePersonName_WhenFullNameIsValid(string input)
        {
            // Act
            var name = new PersonName(input);
            // Assert
            name.FullName.Should().Be(input);
        }
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_ShouldThrowDomainException_WhenNameIsEmpty(string? input)
        {
            // Act
            Action act = () => new PersonName(input!);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Name cannot be empty.");
        }
        [Theory]
        [InlineData("Rodrigo")]
        [InlineData("Furlaneti")]
        public void Constructor_ShouldThrowDomainException_WhenNameHasNoSurname(string input)
        {
            // Act
            Action act = () => new PersonName(input);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Please provide a full name (first and last name).");
        }

        [Fact]
        public void ImplicitOperator_ShouldConvertToStringCorrecty()
        {
            // Arrange
            var input = "Rodrigo Luiz Madeira Furlaneti";
            var personName = new PersonName(input);
            // Act
            string result = personName;
            // Assert
            result.Should().Be(input);
        }

        [Fact]
        public void Constructor_ShouldTrimInputName()
        {
            // Arrange
            var input = "  Rodrigo Furlaneti  ";
            // Act
            var name = new PersonName(input);
            // Assert
            name.FullName.Should().Be("Rodrigo Furlaneti");
        }
    }
}