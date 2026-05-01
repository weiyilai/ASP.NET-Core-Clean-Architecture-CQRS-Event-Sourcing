using System.Collections.Generic;
using Ardalis.Result;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.PublicApi.Extensions;
using Shop.PublicApi.Models;
using Xunit;
using Xunit.Categories;

namespace Shop.UnitTests.Core.Extensions;

[UnitTest]
public class ResultExtensionsTests
{
    [Fact]
    public void ToActionResult_ReturnsOk_ForSuccessResult()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var action = result.ToActionResult();

        // Assert
        action.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)action;
        ok.Value.Should().BeOfType<ApiResponse>();
        var api = (ApiResponse)ok.Value;
        api.Success.Should().BeTrue();
        api.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public void ToActionResult_ReturnsNotFound_ForNotFoundResult()
    {
        // Arrange
        var result = Result.NotFound("not found");

        // Act
        var action = result.ToActionResult();

        // Assert
        action.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)action;
        notFound.Value.Should().BeOfType<ApiResponse>();
        var api = (ApiResponse)notFound.Value;
        api.Success.Should().BeFalse();
        api.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        api.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ToActionResult_ReturnsForbid_ForForbiddenResult()
    {
        // Arrange
        var result = Result.Forbidden();

        // Act
        var action = result.ToActionResult();

        // Assert
        action.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public void ToActionResult_ReturnsUnauthorized_ForUnauthorizedResult()
    {
        // Arrange
        var result = Result.Unauthorized("unauth");

        // Act
        var action = result.ToActionResult();

        // Assert
        action.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorized = (UnauthorizedObjectResult)action;
        unauthorized.Value.Should().BeOfType<ApiResponse>();
        var api = (ApiResponse)unauthorized.Value;
        api.Success.Should().BeFalse();
        api.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public void ToActionResult_GenericCreated_ReturnsCreatedResult()
    {
        // Arrange
        var value = "payload";
        var result = Result<string>.Created(value, location: "/test/1");

        // Act
        var action = result.ToActionResult();

        // Assert
        action.Should().BeOfType<CreatedResult>();
        var created = (CreatedResult)action;
        created.Location.Should().Be("/test/1");
        created.Value.Should().BeOfType(typeof(ApiResponse<string>));
        var api = (ApiResponse<string>)created.Value;
        api.Success.Should().BeTrue();
        api.StatusCode.Should().Be(StatusCodes.Status201Created);
        api.Result.Should().Be(value);
    }

    [Fact]
    public void ToActionResult_GenericOk_ReturnsOkObjectResult()
    {
        // Arrange
        var value = "payload";
        var result = Result<string>.Success(value);

        // Act
        var action = result.ToActionResult();

        // Assert
        action.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)action;
        ok.Value.Should().BeOfType(typeof(ApiResponse<string>));
        var api = (ApiResponse<string>)ok.Value;
        api.Success.Should().BeTrue();
        api.StatusCode.Should().Be(StatusCodes.Status200OK);
        api.Result.Should().Be(value);
    }
}