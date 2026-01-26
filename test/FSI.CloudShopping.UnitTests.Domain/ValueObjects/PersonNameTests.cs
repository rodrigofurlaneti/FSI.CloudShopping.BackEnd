using System;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Core;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class PersonNameTests
    {
        [Fact]
        public void Should_Create_PersonName_When_FullName_Is_Valid()
        {
            // Arrange
            var fullName = "Rodrigo Furlaneti";

            // Act
            var personName = new PersonName(fullName);

            // Assert
            personName.FullName.Should().Be(fullName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Throw_Exception_When_Name_Is_Null_Or_Empty(string invalidName)
        {
            // Act
            Action act = () => new PersonName(invalidName);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage("Name cannot be empty.");
        }

        [Theory]
        [InlineData("Rodrigo")]
        [InlineData("Maria")]
        [InlineData("SingleName")]
        public void Should_Throw_Exception_When_Name_Does_Not_Contain_LastName(string invalidName)
        {
            // Act
            Action act = () => new PersonName(invalidName);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage("Please provide a full name (first and last name).");
        }

        [Fact]
        public void Should_Trim_Extra_Spaces_From_Name()
        {
            // Arrange
            var fullName = "   Rodrigo Furlaneti   ";

            // Act
            var personName = new PersonName(fullName);

            // Assert
            personName.FullName.Should().Be("Rodrigo Furlaneti");
        }

        [Fact]
        public void Should_Convert_PersonName_To_String_Implicitly()
        {
            // Arrange
            var personName = new PersonName("Rodrigo Furlaneti");

            // Act
            string nameAsString = personName;

            // Assert
            nameAsString.Should().Be("Rodrigo Furlaneti");
        }

        [Fact]
        public void ToString_Should_Return_FullName()
        {
            // Arrange
            var personName = new PersonName("Rodrigo Furlaneti");

            // Act
            var result = personName.ToString();

            // Assert
            result.Should().Be("Rodrigo Furlaneti");
        }
    }
}
