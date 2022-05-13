using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models.Aggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

class OrderItemsEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem> {
    public void Configure(EntityTypeBuilder<OrderItem> productConfiguration) {
        productConfiguration.ToTable("orderitems", OrderingContext.DEFAULT_SCHEMA);
        productConfiguration.HasKey(b => b.Id);
        productConfiguration.Ignore(b => b.DomainEvents);
        productConfiguration.Property(b => b.Name);
        productConfiguration.Property(b => b.Quantity);
    }
}
