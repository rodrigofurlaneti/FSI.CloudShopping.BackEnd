using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class EmailTests
    {
        #region Valid Email

        [Theory]
        [InlineData("teste@email.com")]
        [InlineData("user.name@domain.com")]
        [InlineData("user_name@domain.com")]
        [InlineData("user-name@domain.com")]
        public void Constructor_Should_Create_Email_When_Address_Is_Valid(string address)
        {
            // Act
            var email = new Email(address);

            // Assert
            email.Address.Should().Be(address);
        }

        #endregion

        #region Invalid Email

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("email.com")]
        [InlineData("email@")]
        [InlineData("@domain.com")]
        public void Constructor_Should_Throw_When_Address_Is_Invalid(string address)
        {
            Action act = () => new Email(address);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("Invalid email address format.");
        }

        [Fact]
        public void Constructor_Should_Throw_When_Address_Is_Null()
        {
            Action act = () => new Email(null!);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("Invalid email address format.");
        }

        #endregion

        #region Equality (Record)

        [Fact]
        public void Email_Should_Be_Equal_When_Address_Is_Same()
        {
            var email1 = new Email("teste@email.com");
            var email2 = new Email("teste@email.com");

            email1.Should().Be(email2);
            email1.GetHashCode().Should().Be(email2.GetHashCode());
        }

        [Fact]
        public void Email_Should_Not_Be_Equal_When_Address_Is_Different()
        {
            var email1 = new Email("teste@email.com");
            var email2 = new Email("outro@email.com");

            email1.Should().NotBe(email2);
        }

        #endregion

        #region Implicit Operator

        [Fact]
        public void Implicit_Operator_Should_Convert_Email_To_String()
        {
            var email = new Email("teste@email.com");

            string result = email;

            result.Should().Be("teste@email.com");
        }

        #endregion
    }
}
