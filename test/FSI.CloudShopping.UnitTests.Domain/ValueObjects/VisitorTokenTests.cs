using System;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class VisitorTokenTests
    {
        [Fact]
        public void Should_Create_VisitorToken_When_Guid_Is_Valid()
        {
            var guid = Guid.NewGuid();

            var token = new VisitorToken(guid);

            token.Value.Should().Be(guid);
        }

        [Fact]
        public void Should_Throw_Exception_When_Guid_Is_Empty()
        {
            var act = () => new VisitorToken(Guid.Empty);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Visitor token cannot be empty.");
        }

        [Fact]
        public void NewToken_Should_Generate_Valid_Token()
        {
            var token = VisitorToken.NewToken();

            token.Value.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void NewToken_Should_Generate_Different_Tokens()
        {
            var token1 = VisitorToken.NewToken();
            var token2 = VisitorToken.NewToken();

            token1.Value.Should().NotBe(token2.Value);
        }

        [Fact]
        public void ToString_Should_Return_Guid_As_String()
        {
            var guid = Guid.NewGuid();
            var token = new VisitorToken(guid);

            token.ToString().Should().Be(guid.ToString());
        }
    }
}
