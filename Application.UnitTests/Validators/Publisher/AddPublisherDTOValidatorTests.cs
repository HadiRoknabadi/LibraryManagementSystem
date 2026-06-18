using Application.DTOs.Publisher;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;
public class AddPublisherDTOValidatorTests
{
    private readonly AddPublisherDTOValidator _validator = new();

    [Fact]
    public void Name_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new AddPublisherDTO
        {
            Name = string.Empty
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [AutoData]
    public void Name_Should_Have_Error_When_Length_Greater_Than_200(string random)
    {
        // Arrange
        var name = random.PadRight(201, 'a');

        var model = new AddPublisherDTO
        {
            Name = name
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [AutoData]
    public void Name_Should_Not_Have_Error_When_Valid(string random)
    {
        // Arrange
        var name = random.PadRight(50, 'a').Substring(0, 50);

        var model = new AddPublisherDTO
        {
            Name = name
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [AutoData]
    public void PhoneNumber_Should_Have_Error_When_Length_Greater_Than_20(string random)
    {
        // Arrange
        var phone = random.PadRight(21, '1');

        var model = new AddPublisherDTO
        {
            Name = "Test Publisher",
            PhoneNumber = phone
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Theory]
    [AutoData]
    public void PhoneNumber_Should_Not_Have_Error_When_Valid(string random)
    {
        // Arrange
        var phone = random.PadRight(10, '1').Substring(0, 10);

        var model = new AddPublisherDTO
        {
            Name = "Test Publisher",
            PhoneNumber = phone
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Theory]
    [AutoData]
    public void Address_Should_Have_Error_When_Length_Greater_Than_300(string random)
    {
        // Arrange
        var address = random.PadRight(301, 'a');

        var model = new AddPublisherDTO
        {
            Name = "Test Publisher",
            Address = address
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address);
    }

    [Theory]
    [AutoData]
    public void Address_Should_Not_Have_Error_When_Valid(string random)
    {
        // Arrange
        var address = random.PadRight(100, 'a').Substring(0, 100);

        var model = new AddPublisherDTO
        {
            Name = "Test Publisher",
            Address = address
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
    }
}
