using Application.DTOs.Account;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;

public class LoginUserDTOValidatorTests
{
    private readonly LoginUserDTOValidator _validator = new();

    [Fact]
    public void PhoneNumber_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new LoginUserDTO
        {
            PhoneNumber = string.Empty,
            Password = "ValidPass123"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Theory]
    [AutoData]
    public void PhoneNumber_Should_Have_Error_When_Length_Not_11(string random)
    {
        // Arrange
        var phone = random.Length == 11 ? random + "1" : random;
        var model = new LoginUserDTO
        {
            PhoneNumber = phone,
            Password = "ValidPass123"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Theory]
    [AutoData]
    public void PhoneNumber_Should_Have_Error_When_Not_Starting_With_09(string digits)
    {
        // Arrange
        var numbers = new string(digits.Where(char.IsDigit).ToArray());
        numbers = numbers.PadRight(11, '1').Substring(0, 11);
        if (numbers.StartsWith("09"))
            numbers = "08" + numbers.Substring(2);

        var model = new LoginUserDTO
        {
            PhoneNumber = numbers,
            Password = "ValidPass123"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Theory]
    [AutoData]
    public void PhoneNumber_Should_Not_Have_Error_When_Valid(string digits)
    {
        // Arrange
        var onlyDigits = new string(digits.Where(char.IsDigit).ToArray());
        onlyDigits = onlyDigits.PadRight(9, '1').Substring(0, 9);
        var phone = "09" + onlyDigits;

        var model = new LoginUserDTO
        {
            PhoneNumber = phone,
            Password = "ValidPass123"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void Password_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new LoginUserDTO
        {
            PhoneNumber = "09123456789",
            Password = string.Empty
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [AutoData]
    public void Password_Should_Have_Error_When_Length_Less_Than_8(string random)
    {
        // Arrange
        var password = random.Length >= 8 ? random.Substring(0, 7) : random;

        var model = new LoginUserDTO
        {
            PhoneNumber = "09123456789",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [AutoData]
    public void Password_Should_Have_Error_When_Length_Greater_Than_25(string random)
    {
        // Arrange
        var password = random.PadRight(26, 'a');

        var model = new LoginUserDTO
        {
            PhoneNumber = "09123456789",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [AutoData]
    public void Password_Should_Not_Have_Error_When_Length_Valid(string random)
    {
        // Arrange
        var password = random.PadRight(10, 'a').Substring(0, 10);

        var model = new LoginUserDTO
        {
            PhoneNumber = "09123456789",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}
