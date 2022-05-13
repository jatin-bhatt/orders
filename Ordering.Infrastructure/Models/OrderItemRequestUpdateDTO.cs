using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Models {
    public class OrderItemRequestUpdateDTO {
        public Guid ItemId { get; set; }
        public string ProductType { get; set; }

        public int Quantity { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as OrderItemRequestUpdateDTO);
        }

        public bool Equals(OrderItemRequestUpdateDTO other) {
            return other != null &&
                ItemId == other.ItemId &&
                ProductType == other.ProductType &&
                Quantity == other.Quantity;
        }

        public override int GetHashCode() {
            return HashCode.Combine(ItemId, ProductType, Quantity);
        }
    }
}
