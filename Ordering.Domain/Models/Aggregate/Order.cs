using Ordering.Domain.Events;
using Ordering.Domain.Models.Aggregate.Enum;


namespace Ordering.Domain.Models.Aggregate {
    public class Order : Entity<Guid>, IAggregateRoot {
        public OrderStatus OrderStatus { get; private set; }

        public List<OrderItem> OrderItems { get; private set; }
        public DateTime OrderDate { get; private set; }

        public double RequiredBinWidth {
            get {
                return OrderItems.Sum(i => i.Width);
            }
        }

        public Order() {
            OrderItems = new List<OrderItem>();
            OrderDate = DateTime.Now;
        }

        public void SetOrderId(Guid id) {
            Id = id;
        }

        public void AddItem(ProductType type, int quantity) {
            var item = new OrderItem(Guid.NewGuid(), System.Enum.GetName(type.GetType(), type), type, quantity);
            OrderItems.Add(item);
        }

        public void UpdateItem(OrderItem orderItem) {
            var index = OrderItems.FindIndex(i => i.Id == orderItem.Id);
            if (index >= 0) {
                OrderItems[index] = orderItem;
            }
        }

        public void RemoveItems() {
            OrderItems = new List<OrderItem>();
        }

        public void SetOrderShippedStatus() {
            OrderStatus = OrderStatus.Shipped;
            AddDomainEvent(new OrderShippedDomainEvent(this));
        }

        public void SetOrderRecievedStatus() {
            OrderStatus = OrderStatus.Received;
            AddDomainEvent(new OrderRecievedDomainEvent(this));
        }

        public void SetOrderCompletedStatus() {
            OrderStatus = OrderStatus.Completed;
            AddDomainEvent(new OrderCompletedDomainEvent(this));
        }

        public void SetItemSizing() {
            OrderItems = OrderItems.Select(i => { i.SetItemSizing(); return i; }).ToList();
        }
    }
}
