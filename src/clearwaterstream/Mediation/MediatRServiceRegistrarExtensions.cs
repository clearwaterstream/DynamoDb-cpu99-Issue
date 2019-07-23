using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace clearwaterstream.IoC
{
    public static class MediatRServiceRegistrarExtensions
    {
        public static IMediator GetMediator(this ServiceRegistrar serviceRegistrar)
        {
            if (serviceRegistrar == null)
                return null;

            var result = serviceRegistrar.GetInstance<IMediator>();

            return result;
        }
    }
}
