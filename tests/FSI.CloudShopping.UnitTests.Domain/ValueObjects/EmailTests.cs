namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects;

using Xunit;
using FSI.CloudShopping.Domain.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Email_WithValidEmail_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var email = new Email("test@example.com");

        // Assert
        Assert.Equal("test@example.com", email.Value);
    }

    [Fact]
    public void Email_WithInvalidFormat_ShouldThrow()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Email("invalid-email"));
    }

    [Fact]
    public void Email_WithEmptyString_ShouldThrow()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Email(string.Empty));
    }

    [Fact]
    public void Email_ShouldBeCaseInsensitive()
    {
        // Arrange & Act
        var email1 = new Email("Test@Example.com");
        var email2 = new Email("test@example.com");

        // Assert
        Assert.Equal(email1, email2);
    }
}
