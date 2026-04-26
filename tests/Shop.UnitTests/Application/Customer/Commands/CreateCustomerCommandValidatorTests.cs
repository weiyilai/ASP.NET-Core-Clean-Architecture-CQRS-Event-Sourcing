using FluentValidation.TestHelper;
using Shop.Application.Customer.Commands;
using Xunit;
using Xunit.Categories;

namespace Shop.UnitTests.Application.Customer.Commands;

[UnitTest]
public class CreateCustomerCommandValidatorTests
{
    private readonly CreateCustomerCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_WhenFirstNameIsEmpty()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            FirstName = "",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
    }

    [Fact]
    public void Should_HaveError_WhenFirstNameIsTooLong()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            FirstName = new string('A', 101),
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
    }

    [Fact]
    public void Should_HaveError_WhenLastNameIsEmpty()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "",
            Email = "john.doe@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.LastName);
    }

    [Fact]
    public void Should_HaveError_WhenLastNameIsTooLong()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = new string('A', 101),
            Email = "john.doe@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.LastName);
    }

    [Fact]
    public void Should_HaveError_WhenEmailIsEmpty()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = ""
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Should_HaveError_WhenEmailIsInvalid()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Should_HaveError_WhenEmailIsTooLong()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = new string('A', 255) + "@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Should_NotHaveError_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}