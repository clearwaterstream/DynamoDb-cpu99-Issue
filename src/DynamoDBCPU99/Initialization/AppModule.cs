using Autofac;
using clearwaterstream.AWS.Security;
using clearwaterstream.Security;
using DynamoDBCPU99.Application.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDBCPU99.Initialization
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new SecureParameterStoreSecretsContainer()).As<ISecretsContainer>().SingleInstance();

            builder.Register(x => new ZipCodeDb()).AsImplementedInterfaces().SingleInstance();
        }
    }
}
