using Ordering.Domain.Exception;
using Ordering.Domain.Models.Aggregate.Enum;

namespace Ordering.Domain.Models.Aggregate {
    public class OrderItem : Entity<Guid> {

        private double _widthPerQuantity;
        private int _itemPerStack;

        public string Name { get; private set; }
        public ProductType Type { get; private set; }
        public int Quantity { get; private set; }


        public OrderItem() {}

        public OrderItem(Guid id, string name, ProductType type, int quantity) {
            if (quantity < 1) {
                throw new OrderingDomainException(@"Invalid ""Quantity"", must be a positive value");
            }

            Id = id;
            Name = name;
            Type = type;
            Quantity = quantity;

            SetItemSizing();
        }

        public double Width { 
            get { 
                return _widthPerQuantity * (_itemPerStack == 1
                ? Quantity
                : (int)Math.Ceiling((float)Quantity / _itemPerStack));
            } 
        } 

        public void SetItemSizing() {
            switch (Type) {
                case ProductType.PhotoBook:
                    _widthPerQuantity = 19;
                    _itemPerStack = 1;
                    break;
                case ProductType.Calendar:
                    _widthPerQuantity = 10;
                    _itemPerStack = 1;
                    break;
                case ProductType.Canvas:
                    _widthPerQuantity = 16;
                    _itemPerStack = 1;
                    break;
                case ProductType.Cards:
                    _widthPerQuantity = 4.7;
                    _itemPerStack = 1;
                    break;
                case ProductType.Mug:
                    _widthPerQuantity = 94;
                    _itemPerStack = 4;
                    break;
                default:
                    throw new OrderingDomainException("Invalid Type");
            }
        }
    }
}
