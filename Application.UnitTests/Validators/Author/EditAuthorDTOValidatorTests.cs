using Application.DTOs.Author;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;

public class EditAuthorDTOValidatorTests
{
    private readonly EditAuthorDTOValidator _validator = new();

    [Fact]
    public void Name_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new EditAuthorDTO
        {
            Name = string.Empty,
            Family = "TestFamily"
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

        var model = new EditAuthorDTO
        {
            Name = name,
            Family = "TestFamily"
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

        var model = new EditAuthorDTO
        {
            Name = name,
            Family = "TestFamily"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Family_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new EditAuthorDTO
        {
            Name = "TestName",
            Family = string.Empty
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Family);
    }

    [Theory]
    [AutoData]
    public void Family_Should_Have_Error_When_Length_Greater_Than_200(string random)
    {
        // Arrange
        var family = random.PadRight(201, 'a');

        var model = new EditAuthorDTO
        {
            Name = "TestName",
            Family = family
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Family);
    }

    [Theory]
    [AutoData]
    public void Family_Should_Not_Have_Error_When_Valid(string random)
    {
        // Arrange
        var family = random.PadRight(50, 'a').Substring(0, 50);

        var model = new EditAuthorDTO
        {
            Name = "TestName",
            Family = family
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Family);
    }
}
