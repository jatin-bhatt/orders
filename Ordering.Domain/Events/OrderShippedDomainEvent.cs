using MediatR;
using Ordering.Domain.Models.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Events {
    public class OrderShippedDomainEvent : INotification {
        public Order Order { get; }

        public OrderShippedDomainEvent(Order order) {
            this.Order = order;
        }
    }
}
