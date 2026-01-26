using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Tests.Core.Fakes;
using Xunit;
namespace FSI.CloudShopping.Domain.Tests.Core
{
    public class DomainEventTests
    {
        [Fact]
        public void FakeDomainEvent_DeveImplementarIDomainEvent()
        {
            // Arrange
            var domainEvent = new FakeDomainEvent();
            // Act & Assert
            domainEvent.Should().BeAssignableTo<IDomainEvent>();
        }
        [Fact]
        public void IDomainEvent_DeveSerUmaInterface()
        {
            // Act
            var type = typeof(IDomainEvent);
            // Assert
            type.IsInterface.Should().BeTrue();
        }
    }
}
