using System;
using FluentAssertions;
using Shop.Core.SharedKernel;
using Xunit;
using Xunit.Categories;

namespace Shop.UnitTests.Core.SharedKernel;

[UnitTest]
public class BaseEntityTests
{
    [Fact]
    public void Constructor_ShouldGenerateNewGuidForId()
    {
        // Act
        var entity = new TestEntity();

        // Assert
        entity.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Constructor_WithId_ShouldSetSpecifiedId()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var entity = new TestEntity(id);

        // Assert
        entity.Id.Should().Be(id);
    }

    [Fact]
    public void AddDomainEvent_ShouldAddEventToDomainEvents()
    {
        // Arrange
        var entity = new TestEntity();
        var domainEvent = new TestEvent();

        // Act
        entity.AddDomainEvent(domainEvent);

        // Assert
        entity.DomainEvents.Should().Contain(domainEvent);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var entity = new TestEntity();
        entity.AddDomainEvent(new TestEvent());
        entity.AddDomainEvent(new TestEvent());

        // Act
        entity.ClearDomainEvents();

        // Assert
        entity.DomainEvents.Should().BeEmpty();
    }

    private class TestEntity : BaseEntity
    {
        public TestEntity() { }
        public TestEntity(Guid id) : base(id) { }

        public new void AddDomainEvent(BaseEvent domainEvent) => base.AddDomainEvent(domainEvent);
    }

    private class TestEvent : BaseEvent
    {
        public TestEvent()
        {
            MessageType = "TestEvent";
            AggregateId = Guid.NewGuid();
        }
    }
}