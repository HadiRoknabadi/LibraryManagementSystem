using Application.DTOs.Book;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;

public class AddBookDTOValidatorTests
{
    private readonly AddBookDTOValidator _validator = new();

    [Fact]
    public void Title_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new AddBookDTO
        {
            Title = string.Empty,
            ISBN = "1234567890"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [AutoData]
    public void Title_Should_Have_Error_When_Length_Greater_Than_300(string random)
    {
        // Arrange
        var title = random.PadRight(301, 'a');

        var model = new AddBookDTO
        {
            Title = title,
            ISBN = "1234567890"
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
        var title = random.PadRight(100, 'a').Substring(0, 100);

        var model = new AddBookDTO
        {
            Title = title,
            ISBN = "1234567890"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void ISBN_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new AddBookDTO
        {
            Title = "Test Book",
            ISBN = string.Empty
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ISBN);
    }

    [Theory]
    [AutoData]
    public void ISBN_Should_Have_Error_When_Length_Greater_Than_20(string random)
    {
        // Arrange
        var isbn = random.PadRight(21, '1');

        var model = new AddBookDTO
        {
            Title = "Test Book",
            ISBN = isbn
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ISBN);
    }

    [Theory]
    [AutoData]
    public void ISBN_Should_Not_Have_Error_When_Valid(string random)
    {
        // Arrange
        var isbn = random.PadRight(10, '1').Substring(0, 10);

        var model = new AddBookDTO
        {
            Title = "Test Book",
            ISBN = isbn
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ISBN);
    }
}
