using Application.DTOs.Borrowing;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;

public class EditBorrowDTOValidatorTests
{
    private readonly EditBorrowDTOValidator _validator = new();

    [Fact]
    public void DueDate_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new EditBorrowDTO
        {
            DueDate = string.Empty
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DueDate);
    }

    [Theory]
    [AutoData]
    public void DueDate_Should_Have_Error_When_Length_Greater_Than_10(string random)
    {
        // Arrange
        var dueDate = random.PadRight(11, '1');

        var model = new EditBorrowDTO
        {
            DueDate = dueDate
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DueDate);
    }

    [Theory]
    [AutoData]
    public void DueDate_Should_Not_Have_Error_When_Valid(string random)
    {
        // Arrange
        var dueDate = random.PadRight(10, '1').Substring(0, 10);

        var model = new EditBorrowDTO
        {
            DueDate = dueDate
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.DueDate);
    }
}
