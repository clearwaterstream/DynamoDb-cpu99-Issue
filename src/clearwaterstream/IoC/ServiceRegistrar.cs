using Autofac;
using clearwaterstream.Mediation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace clearwaterstream.IoC
{
    /// <summary>
    /// IoC pattern implemintation based on CommonServiceLocator by Microsoft
    /// </summary>
    public class ServiceRegistrar : IDisposable
    {
        private static ServiceRegistrar _current = null;
        private static readonly object _currentLock = new object();

        private readonly IContainer _container = null;
        static IServiceProvider _serviceProvider = null;

        static IConfiguration _configuration = null;

        private ServiceRegistrar(IContainer container)
        {
            _container = container;
        }

        /// <param name="services">These are application scoped services, could be ASP.NET core services, etc</param>
        /// <param name="performRegistrations">action that would perform registrations, etc</param>
        public static ServiceRegistrar Bootstrap(Action<ContainerBuilder> performRegistrations)
        {
            if (_current == null)
            {
                lock (_currentLock)
                {
                    if (_current == null)
                    {
                        var builder = new ContainerBuilder();

                        builder.RegisterMediatR();

                        performRegistrations?.Invoke(builder);

                        var container = builder.Build();

                        _current = new ServiceRegistrar(container);
                    }
                }
            }

            return _current;
        }

        public TService GetInstance<TService>()
        {
            var result = _container.Resolve<TService>();

            return result;
        }

        public bool TryGetInstance<TService>(out TService instance)
        {
            var result = _container.TryResolve(out instance);

            return result;
        }

        public bool TryGetInstance(Type serviceType, out object instance)
        {
            var result = _container.TryResolve(serviceType, out instance);

            return result;
        }

        public TService GetInstance<TService>(string registrationKey)
        {
            var found = _container.ResolveNamed<TService>(registrationKey);

            return found;
        }

        public bool TryGetInstance<TService>(string registrationKey, out TService instance)
        {
            var found = _container.TryResolveNamed(registrationKey, typeof(TService), out object tmpInstance);

            instance = found ? (TService)tmpInstance : default(TService);

            return found;
        }

        public static TService GetService<TService>()
        {
            return (TService)_serviceProvider.GetService(typeof(TService));
        }

        public static TService GetHttpClient<TService>()
        {
            return (TService)_serviceProvider.GetService(typeof(TService));
        }

        // create a method to make assginment more explicit (thus avoiding accidentaly setting this)
        public static void SetServiceProvider(IServiceProvider services)
        {
            _serviceProvider = services;
        }

        public static void SetConfigutation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static IConfiguration Configuration => _configuration;

        public static IServiceProvider Services => _serviceProvider;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_currentLock)
                {
                    _configuration = null;
                    _serviceProvider = null;
                    _container?.Dispose();
                    _current = null;
                }
            }
        }

        public static ServiceRegistrar Current { get { return _current; } }
    }
}
