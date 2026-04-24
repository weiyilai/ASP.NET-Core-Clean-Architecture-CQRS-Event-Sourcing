using System.Linq;
using FluentAssertions;
using Shop.Core.Extensions;
using Xunit;
using Xunit.Categories;

namespace Shop.UnitTests.Core.Extensions;

[UnitTest]
public class AssemblyExtensionsTests
{
    [Fact]
    public void GetAllTypesOf_ShouldReturnTypesImplementingInterface()
    {
        // Arrange
        var assembly = typeof(AssemblyExtensionsTests).Assembly;

        // Act
        var types = assembly.GetAllTypesOf<object>();

        // Assert
        types.Should().NotBeNull();
        types.Should().Contain(typeof(AssemblyExtensionsTests));
    }
}