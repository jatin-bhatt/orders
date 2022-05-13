using Ordering.Domain.Models.Aggregate;
using Xunit;


namespace Ordering.Domain.Tests.Models.Aggregate {
    public class OrderItemTests {

        [Theory]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Calendar, 1, 10)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Calendar, 2, 20)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Calendar, 4, 40)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Calendar, 7, 70)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Calendar, 11, 110)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Calendar, 6, 60)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Calendar, 12, 120)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.PhotoBook, 1, 19)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.PhotoBook, 2, 38)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.PhotoBook, 4, 76)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.PhotoBook, 7, 133)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.PhotoBook, 11, 209)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.PhotoBook, 6, 114)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.PhotoBook, 12, 228)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Mug, 1, 94)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Mug, 2, 94)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Mug, 4, 94)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Mug, 7, 188)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Mug, 11, 282)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Mug, 6, 188)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Mug, 12, 282)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Canvas, 1, 16)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Canvas, 2, 32)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Canvas, 4, 64)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Canvas, 7, 112)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Canvas, 11, 176)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Canvas, 6, 96)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Canvas, 12, 192)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Cards, 1, 4.7)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Cards, 2, 9.4)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Cards, 4, 18.8)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Cards, 7, 32.9)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Cards, 11, 51.7)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Cards, 6, 28.2)]
        [InlineData(Domain.Models.Aggregate.Enum.ProductType.Cards, 12, 56.4)]

        public void OrderItemCreated_SuppliedTypeAndQuantityt_ReturnsAppropriateWidthForItem(Domain.Models.Aggregate.Enum.ProductType productType, int quantity, double expectedWidth) {
            // Arrange
            Guid fakeOrderItemId = Guid.NewGuid();
            

            // Act
            OrderItem item = new OrderItem(
                fakeOrderItemId,
                "fake name",
                productType,
                quantity);

            // Assert
            Assert.Equal(expectedWidth, Math.Round(item.Width, 2));
        }

    }
}
