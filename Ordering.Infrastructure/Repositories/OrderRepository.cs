using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Models;
using Ordering.Domain.Models.Aggregate;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository {
    private readonly OrderingContext _context;

    public IUnitOfWork UnitOfWork {
        get {
            return _context;
        }
    }

    public OrderRepository(OrderingContext context) {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Order> AddAsync(Order order) {
        return (await _context.Orders.AddAsync(order)).Entity;
    }

    public async Task<Order> GetAsync(Guid orderId) {
        var order = await _context
                            .Orders
                            .Include(o => o.OrderItems)
                            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null) {
            order = _context
                        .Orders
                        .Local
                        .FirstOrDefault(o => o.Id == orderId);
        }
        if (order != null) {
            await _context.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();
            order.SetItemSizing();
        }
        return order;
    }

    public async Task UpdateAsync(Order order) {
        _context.Entry(order).State = EntityState.Modified;
    }
}
