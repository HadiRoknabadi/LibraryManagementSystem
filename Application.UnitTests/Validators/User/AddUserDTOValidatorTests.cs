using Application.DTOs.User;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;

public class AddUserDTOValidatorTests
{
    private readonly AddUserDTOValidator _validator = new();

    [Fact]
    public void Name_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new AddUserDTO
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

        var model = new AddUserDTO
        {
            Name = name
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Family_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new AddUserDTO
        {
            Name = "Test",
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

        var model = new AddUserDTO
        {
            Name = "Test",
            Family = family
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Family);
    }

    [Fact]
    public void PhoneNumber_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new AddUserDTO
        {
            Name = "Test",
            Family = "User",
            PhoneNumber = string.Empty
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
        var phone = random.PadRight(10, '1');

        var model = new AddUserDTO
        {
            Name = "Test",
            Family = "User",
            PhoneNumber = phone
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void PhoneNumber_Should_Have_Error_When_Format_Invalid()
    {
        // Arrange
        var model = new AddUserDTO
        {
            Name = "Test",
            Family = "User",
            PhoneNumber = "08123456789"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Theory]
    [AutoData]
    public void MembershipCode_Should_Have_Error_When_Length_Greater_Than_20(string random)
    {
        // Arrange
        var code = random.PadRight(21, 'a');

        var model = new AddUserDTO
        {
            Name = "Test",
            Family = "User",
            PhoneNumber = "09123456789",
            MembershipCode = code
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MembershipCode);
    }

    [Fact]
    public void Password_Should_Have_Error_When_Empty()
    {
        // Arrange
        var model = new AddUserDTO
        {
            Name = "Test",
            Family = "User",
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
        var pass = random.Substring(0, 6);

        var model = new AddUserDTO
        {
            Name = "Test",
            Family = "User",
            PhoneNumber = "09123456789",
            Password = pass
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
        var pass = random.PadRight(26, 'a');

        var model = new AddUserDTO
        {
            Name = "Test",
            Family = "User",
            PhoneNumber = "09123456789",
            Password = pass
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void PhoneNumber_Should_Not_Have_Error_When_Valid()
    {
        // Arrange
        var model = new AddUserDTO
        {
            Name = "Test",
            Family = "User",
            PhoneNumber = "09123456789",
            Password = "12345678"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }
}
