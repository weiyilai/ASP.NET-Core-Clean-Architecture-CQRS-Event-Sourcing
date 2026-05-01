using System;
using FluentAssertions;
using Shop.Core.SharedKernel;
using Xunit;
using Xunit.Categories;

namespace Shop.UnitTests.Core.SharedKernel;

[UnitTest]
public class EventStoreTests
{
    [Fact]
    public void Constructor_WithParameters_ShouldSetProperties()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        const string messageType = "TestMessage";
        const string data = "TestData";

        // Act
        var eventStore = new EventStore(aggregateId, messageType, data);

        // Assert
        eventStore.AggregateId.Should().Be(aggregateId);
        eventStore.MessageType.Should().Be(messageType);
        eventStore.Data.Should().Be(data);
        eventStore.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void DefaultConstructor_ShouldInitializeProperties()
    {
        // Act
        var eventStore = new EventStore();

        // Assert
        eventStore.Id.Should().NotBe(Guid.Empty);
        eventStore.Data.Should().BeNull();
        eventStore.MessageType.Should().BeNull();
        eventStore.AggregateId.Should().Be(Guid.Empty);
    }
}