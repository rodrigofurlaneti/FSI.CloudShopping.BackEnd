using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.Tests.Core.Fakes;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.Core
{
    public class EntityTests
    {
        [Fact]
        public void CreatedAt_DeveSerInicializadoComDataAtual()
        {
            // Arrange
            var before = DateTime.Now;

            // Act
            var entity = new FakeEntity(1);

            var after = DateTime.Now;

            // Assert
            entity.CreatedAt.Should().BeOnOrAfter(before)
                            .And.BeOnOrBefore(after);
        }

        [Fact]
        public void Equals_DeveRetornarTrue_QuandoCompararAMesmaReferencia()
        {
            // Arrange
            var entity = new FakeEntity(1);

            // Act
            var result = entity.Equals(entity);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_DeveRetornarFalse_QuandoObjetoForNulo()
        {
            // Arrange
            var entity = new FakeEntity(1);

            // Act
            var result = entity.Equals(null);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_DeveRetornarTrue_QuandoIdsForemIguais()
        {
            // Arrange
            var entity1 = new FakeEntity(1);
            var entity2 = new FakeEntity(1);

            // Act
            var result = entity1.Equals(entity2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_DeveRetornarFalse_QuandoIdsForemDiferentes()
        {
            // Arrange
            var entity1 = new FakeEntity(1);
            var entity2 = new FakeEntity(2);

            // Act
            var result = entity1.Equals(entity2);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_DeveSerIgual_QuandoIdsForemIguais()
        {
            // Arrange
            var entity1 = new FakeEntity(1);
            var entity2 = new FakeEntity(1);

            // Act
            var hash1 = entity1.GetHashCode();
            var hash2 = entity2.GetHashCode();

            // Assert
            hash1.Should().Be(hash2);
        }

        [Fact]
        public void GetHashCode_DeveSerDiferente_QuandoIdsForemDiferentes()
        {
            // Arrange
            var entity1 = new FakeEntity(1);
            var entity2 = new FakeEntity(2);

            // Act
            var hash1 = entity1.GetHashCode();
            var hash2 = entity2.GetHashCode();

            // Assert
            hash1.Should().NotBe(hash2);
        }
    }
}
