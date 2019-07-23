using Autofac;
using clearwaterstream.IoC;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDBCPU99.Initialization
{
    public static class AppInitializer
    {
        public static void Initialize(IConfiguration configuration)
        {
            ServiceRegistrar.SetConfigutation(configuration);

            ServiceRegistrar.Bootstrap(PerformRegistrations);
        }

        static void PerformRegistrations(ContainerBuilder builder)
        {
            builder.RegisterModule(new AppModule());
        }
    }
}
