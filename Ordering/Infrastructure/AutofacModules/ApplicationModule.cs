using Autofac;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure.Repositories;

namespace Ordering.API.Infrastructure.AutofacModules;

public class ApplicationModule : Module {
    public string QueriesConnectionString { get; }

    public ApplicationModule(string qconstr) {
        QueriesConnectionString = qconstr;
    }

    protected override void Load(ContainerBuilder builder) {
        builder.RegisterType<OrderRepository>()
            .As<IOrderRepository>()
            .InstancePerLifetimeScope();

    }
}
