using Autofac;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace clearwaterstream.Mediation
{
    public static class MediatRAutofacExtensions
    {
        public static void RegisterMediatR(this ContainerBuilder builder)
        {
            if (builder == null)
                return;

            builder
              .RegisterType<Mediator>()
              .As<IMediator>()
              .SingleInstance();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();

                return t => ResolveService(c, t);
            });
        }

        static object ResolveService(IComponentContext componentContext, Type serviceType)
        {
            object service = null;

            try
            {
                service = componentContext.Resolve(serviceType);
            }
            catch (Exception ex)
            {
                throw new Exception($"cannot resolve service of type {serviceType.Name}", ex);
            }

            return service;
        }
    }
}
