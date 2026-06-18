using Application.DTOs.BookCopy;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;

public class EditBookCopyDTOValidatorTests
{
    private readonly EditBookCopyDTOValidator _validator = new();

    [Theory]
    [AutoData]
    public void ShelfLocation_Should_Have_Error_When_Length_Greater_Than_100(string random)
    {
        // Arrange
        var shelfLocation = random.PadRight(101, 'a');

        var model = new EditBookCopyDTO
        {
            ShelfLocation = shelfLocation
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShelfLocation);
    }

    [Theory]
    [AutoData]
    public void ShelfLocation_Should_Not_Have_Error_When_Valid(string random)
    {
        // Arrange
        var shelfLocation = random.PadRight(50, 'a').Substring(0, 50);

        var model = new EditBookCopyDTO
        {
            ShelfLocation = shelfLocation
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ShelfLocation);
    }
}
