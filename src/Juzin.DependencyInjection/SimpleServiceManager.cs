using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Juzin.DependencyInjection
{
    /// <summary>
    /// Simple Microsoft.Extensions.DependencyInjection wrapper combining ServiceCollection, ServiceProvider, ConfigurationBuilder
    /// </summary>
    public class SimpleServiceManager : ISimpleServiceManager
    {
        #region Private fields
        private readonly IServiceCollection _serviceCollection;
        private readonly IConfigurationBuilder _configurationBuilder;
        private IServiceProvider _serviceProvider;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="SimpleServiceManager"/>
        /// </summary>
        public SimpleServiceManager()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddSingleton<ISimpleServiceManager>(this);
            _configurationBuilder = new ConfigurationBuilder();
        }
        #endregion

        #region ISimpleServiceManager members
        /// <inheritdoc/>
        public ISimpleServiceManager AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            IsServicesProviderNotBuild();
            _serviceCollection.AddSingleton<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddSingleton<TService>()
            where TService : class
        {
            IsServicesProviderNotBuild();
            _serviceCollection.AddSingleton<TService>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddSingleton<TService>(TService implementationInstance)
            where TService : class
        {
            IsServicesProviderNotBuild();
            _serviceCollection.AddSingleton<TService>(implementationInstance);
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            IsServicesProviderNotBuild();
            _serviceCollection.AddTransient<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddTransient<TService>()
            where TService : class
        {
            IsServicesProviderNotBuild();
            _serviceCollection.AddTransient<TService>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            IsServicesProviderNotBuild();
            _serviceCollection.AddScoped<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddScoped<TService>()
            where TService : class
        {
            IsServicesProviderNotBuild();
            _serviceCollection.AddScoped<TService>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager Configure(Action<IConfigurationBuilder> configureBuilderAction)
        {
            if (configureBuilderAction is null)
            {
                throw new ArgumentNullException(nameof(configureBuilderAction));
            }

            IsServicesProviderNotBuild();
            configureBuilderAction.Invoke(_configurationBuilder);
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager ConfigureServices(Action<IServiceCollection> serviceCollectionAction)
        {
            if (serviceCollectionAction is null)
            {
                throw new ArgumentNullException(nameof(serviceCollectionAction));
            }
            IsServicesProviderNotBuild();
            serviceCollectionAction.Invoke(_serviceCollection);
            return this;
        }

        /// <inheritdoc/>
        public void BuildServiceProvider()
        {
            IsServicesProviderNotBuild();
            _serviceCollection.AddSingleton<IConfiguration>(_configurationBuilder.Build());
            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        /// <inheritdoc/>
        public TService GetRequiredService<TService>()
            where TService : class
        {
            IsServiceProviderBuild();
            return _serviceProvider.GetRequiredService<TService>();
        }

        /// <inheritdoc/>
        public TService GetService<TService>()
            where TService : class
        {
            IsServiceProviderBuild();
            return _serviceProvider.GetService<TService>();
        }
        #endregion

        #region Private members
        private void IsServicesProviderNotBuild()
        {
            if (_serviceProvider != null)
            {
                throw new InvalidOperationException("Service provider is already build. You cannot add additional services or setup any configuration.");
            }
        }

        private void IsServiceProviderBuild()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not build. Call BuildServiceProvider() before calling get service from container.");
            }
        }
        #endregion
    }
}
