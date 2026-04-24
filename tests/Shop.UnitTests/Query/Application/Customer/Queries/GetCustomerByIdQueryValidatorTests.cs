using System;
using FluentValidation.TestHelper;
using Shop.Query.Application.Customer.Queries;
using Xunit;
using Xunit.Categories;

namespace Shop.UnitTests.Query.Application.Customer.Queries;

[UnitTest]
public class GetCustomerByIdQueryValidatorTests
{
    private readonly GetCustomerByIdQueryValidator _validator = new();

    [Fact]
    public void Should_HaveError_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetCustomerByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }

    [Fact]
    public void Should_NotHaveError_WhenIdIsValid()
    {
        // Arrange
        var query = new GetCustomerByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}