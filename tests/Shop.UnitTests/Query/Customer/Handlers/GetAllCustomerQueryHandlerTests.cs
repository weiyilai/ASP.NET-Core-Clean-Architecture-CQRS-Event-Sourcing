using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
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
public class GetAllCustomerQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WithCustomers()
    {
        // Arrange
        var customers = new List<CustomerQueryModel>
        {
            new(Guid.NewGuid(), "John", "Doe", "Male", "john.doe@example.com", DateTime.Now),
            new(Guid.NewGuid(), "Jane", "Smith", "Female", "jane.smith@example.com", DateTime.Now)
        };

        var repository = Substitute.For<ICustomerReadOnlyRepository>();
        repository.GetAllAsync().Returns(Task.FromResult<IEnumerable<CustomerQueryModel>>(customers));

        var cacheService = Substitute.For<ICacheService>();
        cacheService.GetOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<CustomerQueryModel>>>>())
            .Returns(Task.FromResult<IEnumerable<CustomerQueryModel>>(customers));

        var handler = new GetAllCustomerQueryHandler(repository, cacheService);
        var query = new GetAllCustomerQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(customers);
    }
}