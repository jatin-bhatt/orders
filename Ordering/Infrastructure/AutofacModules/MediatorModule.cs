﻿using Autofac;
using MediatR;
using System.Reflection;

namespace Ordering.API.Infrastructure.AutofacModules;

public class MediatorModule : Autofac.Module {
    protected override void Load(ContainerBuilder builder) {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();


        builder.Register<ServiceFactory>(context => {
            var componentContext = context.Resolve<IComponentContext>();
            return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
        });

    }
}
