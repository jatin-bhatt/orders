using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Models {
    public class OrderItemRequestDTO {
        public string ProductType { get; set; }

        public int Quantity { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as OrderItemRequestDTO);
        }

        public bool Equals(OrderItemRequestDTO other) {
            return other != null &&
                   ProductType == other.ProductType &&
                   Quantity == other.Quantity;
        }

        public override int GetHashCode() {
            return HashCode.Combine(ProductType, Quantity);
        }
    }
}
