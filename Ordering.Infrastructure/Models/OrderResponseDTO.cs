using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Models {
    public class OrderResponseDTO {
        public Guid OrderID { get; set; }
        public List<OrderItemRequestDTO> OrderItems { get; set; }
        public double RequiredBinWidth { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as OrderResponseDTO);
        }

        public bool Equals(OrderResponseDTO other) {
            return other != null &&
                   OrderID == other.OrderID &&
                   OrderItems.SequenceEqual(other.OrderItems) &&
                   RequiredBinWidth == other.RequiredBinWidth;
        }

        public override int GetHashCode() {
            return HashCode.Combine(OrderID, OrderItems, RequiredBinWidth);
        }
    }
}
