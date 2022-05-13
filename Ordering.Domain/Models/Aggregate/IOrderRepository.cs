using Ordering.Domain.Models;
using Ordering.Domain.Models.Aggregate;

namespace Ordering.Domain.AggregatesModel.OrderAggregate;

public interface IOrderRepository : IRepository<Order> {
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task<Order> GetAsync(Guid orderId);
}
