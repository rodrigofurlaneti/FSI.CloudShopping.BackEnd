using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class PasswordTests
    {
        #region Constructor - Valid

        [Fact]
        public void Constructor_Should_Create_Password_When_Hash_Is_Valid()
        {
            // Arrange
            var hash = "a1b2c3d4e5f6g7h8i9j0k"; // >= 20 chars

            // Act
            var password = new Password(hash);

            // Assert
            password.Hash.Should().Be(hash);
        }

        #endregion

        #region Constructor - Invalid (Empty / Null)

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_Hash_Is_Empty(string hash)
        {
            Action act = () => new Password(hash);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("Password hash is required.");
        }

        [Fact]
        public void Constructor_Should_Throw_When_Hash_Is_Null()
        {
            Action act = () => new Password(null!);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("Password hash is required.");
        }

        #endregion

        #region Constructor - Invalid (Too Short)

        [Theory]
        [InlineData("short-hash")]
        [InlineData("1234567890123456789")] // 19 chars
        public void Constructor_Should_Throw_When_Hash_Is_Too_Short(string hash)
        {
            Action act = () => new Password(hash);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("Invalid password hash format.");
        }

        #endregion

        #region Equality (Record)

        [Fact]
        public void Password_Should_Be_Equal_When_Hash_Is_Same()
        {
            var hash = "a1b2c3d4e5f6g7h8i9j0k";

            var password1 = new Password(hash);
            var password2 = new Password(hash);

            password1.Should().Be(password2);
            password1.GetHashCode().Should().Be(password2.GetHashCode());
        }

        [Fact]
        public void Password_Should_Not_Be_Equal_When_Hash_Is_Different()
        {
            var password1 = new Password("a1b2c3d4e5f6g7h8i9j0k");
            var password2 = new Password("z9y8x7w6v5u4t3s2r1q0");

            password1.Should().NotBe(password2);
        }

        #endregion

        #region ToString

        [Fact]
        public void ToString_Should_Return_Hash()
        {
            var hash = "a1b2c3d4e5f6g7h8i9j0k";
            var password = new Password(hash);

            password.ToString().Should().Be(hash);
        }

        #endregion
    }
}
