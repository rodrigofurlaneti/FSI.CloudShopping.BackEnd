using FluentAssertions;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Services;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using Moq;
using Xunit;

namespace FSI.CloudShopping.UnitTests.Application.Services
{
    public class CustomerAppServiceTests
    {
        [Fact]
        public async Task Register_ShouldReturnDto_WhenDataIsValid()
        {
            // Arrange
            var repoMock = new Mock<ICustomerRepository>();
            var service = new CustomerAppService(repoMock.Object);
            var dto = new CustomerDTO
            {
                Name = "Rodrigo Furlaneti",
                Email = "rodrigo@furlaneti.com",
                TaxId = "50269324005",
                Phone = "11999999999"
            };

            // Act
            var result = await service.RegisterAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.IsCompany.Should().BeFalse();
            repoMock.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        }
    }
}
