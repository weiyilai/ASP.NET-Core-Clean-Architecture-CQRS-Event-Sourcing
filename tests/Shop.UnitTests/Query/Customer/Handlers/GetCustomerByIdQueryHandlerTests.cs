using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using Shop.Core.SharedKernel;
using Shop.Query.Application.Customer.Handlers;
using Shop.Query.Application.Customer.Queries;
using Shop.Query.Data.Repositories.Abstractions;
using Shop.Query.QueriesModel;
using Xunit;
using Xunit.Categories;

namespace Shop.UnitTests.Query.Customer.Handlers;

[UnitTest]
public class GetCustomerByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidId_ShouldReturnSuccessResult_WithCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new CustomerQueryModel(customerId, "John", "Doe", "Male", "john.doe@example.com", DateTime.Now);

        var validator = Substitute.For<IValidator<GetCustomerByIdQuery>>();
        validator.ValidateAsync(Arg.Any<GetCustomerByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var repository = Substitute.For<ICustomerReadOnlyRepository>();
        repository.GetByIdAsync(customerId).Returns(Task.FromResult(customer));

        var cacheService = Substitute.For<ICacheService>();
        cacheService.GetOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<CustomerQueryModel>>>())
            .Returns(Task.FromResult(customer));

        var handler = new GetCustomerByIdQueryHandler(validator, repository, cacheService);
        var query = new GetCustomerByIdQuery(customerId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task Handle_ValidId_NotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var validator = Substitute.For<IValidator<GetCustomerByIdQuery>>();
        validator.ValidateAsync(Arg.Any<GetCustomerByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var repository = Substitute.For<ICustomerReadOnlyRepository>();
        repository.GetByIdAsync(customerId).Returns(Task.FromResult<CustomerQueryModel>(null));

        var cacheService = Substitute.For<ICacheService>();
        cacheService.GetOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<CustomerQueryModel>>>())
            .Returns(Task.FromResult<CustomerQueryModel>(null));

        var handler = new GetCustomerByIdQueryHandler(validator, repository, cacheService);
        var query = new GetCustomerByIdQuery(customerId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(ResultStatus.NotFound);
        result.Errors.Should().Contain($"No customer found by Id: {customerId}");
    }

    [Fact]
    public async Task Handle_InvalidId_ShouldReturnInvalidResult()
    {
        // Arrange
        var customerId = Guid.Empty;
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Id", "Id cannot be empty") });

        var validator = Substitute.For<IValidator<GetCustomerByIdQuery>>();
        validator.ValidateAsync(Arg.Is<GetCustomerByIdQuery>(q => q.Id == Guid.Empty), Arg.Any<CancellationToken>())
            .Returns(validationResult);

        var repository = Substitute.For<ICustomerReadOnlyRepository>();
        var cacheService = Substitute.For<ICacheService>();

        var handler = new GetCustomerByIdQueryHandler(validator, repository, cacheService);
        var query = new GetCustomerByIdQuery(customerId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(ResultStatus.Invalid);
    }
}