using Ordering.Domain.Models.Aggregate;
using Xunit;


namespace Ordering.Domain.Tests.Models.Aggregate {
    public class OrderTests {
        [Fact]
        public void SetOrderId_SetsSuppliedOrderId() {
            // Arrange
            Guid fakeOrderId = Guid.NewGuid();
            Order order = new Order();

            // Act
            order.SetOrderId(fakeOrderId);

            // Assert
            Assert.Equal(fakeOrderId, order.Id);
        }

        [Fact]
        public void AddItem_AddsSuppliedItemTotheDomainEntity() {
            // Arrange
            Guid fakeOrderId = Guid.NewGuid();
            Order order = new Order();

            // Act
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Canvas, 10);

            // Assert
            Assert.NotEmpty(order.OrderItems);
        }

        [Fact]
        public void UpdateItem_UpdatesSuppliedItem() {
            // Arrange
            Guid fakeOrderId = Guid.NewGuid();
            Order order = new Order();
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Canvas, 10);

            OrderItem item = new OrderItem(order.OrderItems.First().Id, "updated product", Domain.Models.Aggregate.Enum.ProductType.Mug, 4);

            // Act
            order.UpdateItem(item);

            // Assert
            Assert.Equal(item, order.OrderItems.First());
        }

        [Fact]
        public void RemoveItems_EmptiesLists() {
            // Arrange
            Guid fakeOrderId = Guid.NewGuid();
            Order order = new Order();
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Canvas, 10);

            // Act
            order.RemoveItems();

            // Assert
            Assert.Empty(order.OrderItems);
        }


        [Fact]
        public void SetOrderShippedStatus_SetsAppropriateStatus() {
            // Arrange
            Guid fakeOrderId = Guid.NewGuid();
            Order order = new Order();
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Canvas, 10);

            // Act
            order.SetOrderShippedStatus();

            // Assert
            Assert.Equal(Domain.Models.Aggregate.Enum.OrderStatus.Shipped , order.OrderStatus);
        }

        [Fact]
        public void SetOrderRecievedStatus_SetsAppropriateStatus() {
            // Arrange
            Guid fakeOrderId = Guid.NewGuid();
            Order order = new Order();
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Canvas, 10);

            // Act
            order.SetOrderRecievedStatus();

            // Assert
            Assert.Equal(Domain.Models.Aggregate.Enum.OrderStatus.Received, order.OrderStatus);
        }

        [Fact]
        public void SetOrderCompletedStatus_SetsAppropriateStatus() {
            // Arrange
            Guid fakeOrderId = Guid.NewGuid();
            Order order = new Order();
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Canvas, 10);

            // Act
            order.SetOrderCompletedStatus();

            // Assert
            Assert.Equal(Domain.Models.Aggregate.Enum.OrderStatus.Completed, order.OrderStatus);
        }
    }
}
