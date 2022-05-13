namespace Ordering.Infrastructure.Models {
    public class OrderRequestDTO {
        public Guid OrderID { get; set; }
        public List<OrderItemRequestDTO> OrderItems { get; set; }


        public override bool Equals(object obj) {
            return Equals(obj as OrderResponseDTO);
        }

        public bool Equals(OrderResponseDTO other) {
            return other != null &&
                   OrderID == other.OrderID &&
                   OrderItems.SequenceEqual(other.OrderItems);
        }

        public override int GetHashCode() {
            return HashCode.Combine(OrderID, OrderItems);
        }
    }
}
