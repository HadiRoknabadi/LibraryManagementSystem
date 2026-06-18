using Application.DTOs.BookCategory;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;

public class EditBookCategoryDTOValidatorTests
{
    private readonly EditBookCategoryDTOValidator _validator = new();

    [Fact]
    public void Title_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new EditBookCategoryDTO
        {
            Title = string.Empty
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [AutoData]
    public void Title_Should_Have_Error_When_Length_Greater_Than_150(string random)
    {
        // Arrange
        var title = random.PadRight(151, 'a');

        var model = new EditBookCategoryDTO
        {
            Title = title
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [AutoData]
    public void Title_Should_Not_Have_Error_When_Valid(string random)
    {
        // Arrange
        var title = random.PadRight(50, 'a').Substring(0, 50);

        var model = new EditBookCategoryDTO
        {
            Title = title
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }
}
