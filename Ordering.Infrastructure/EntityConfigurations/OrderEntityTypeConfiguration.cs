using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models.Aggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order> {
    public void Configure(EntityTypeBuilder<Order> orderConfiguration) {
        orderConfiguration.ToTable("orders", OrderingContext.DEFAULT_SCHEMA);
        orderConfiguration.HasKey(b => b.Id);
        orderConfiguration.Ignore(b => b.DomainEvents);
        var navigation = orderConfiguration.Metadata.FindNavigation(nameof(Order.OrderItems));
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
