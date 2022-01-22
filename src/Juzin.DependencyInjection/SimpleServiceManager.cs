using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Juzin.DependencyInjection
{
    /// <summary>
    /// Simple Microsoft.Extensions.DependencyInjection wrapper combining ServiceCollection, ServiceProvider, ConfigurationBuilder
    /// </summary>
    public class SimpleServiceManager : ISimpleServiceManager, IDisposable, IAsyncDisposable
    {
        #region Private fields
        private readonly IServiceCollection _serviceCollection;
        private readonly IConfigurationBuilder _configurationBuilder;
        private ServiceProvider _serviceProvider;
        private bool _disposed;
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
            _serviceCollection.AddSingleton(implementationInstance);
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
        public IConfigurationRoot Configure(Action<IConfigurationBuilder> configureBuilderAction)
        {
            if (configureBuilderAction is null)
            {
                throw new ArgumentNullException(nameof(configureBuilderAction));
            }

            IsServicesProviderNotBuild();
            configureBuilderAction.Invoke(_configurationBuilder);
            var configuration = _configurationBuilder.Build();
            _serviceCollection.AddSingleton<IConfiguration>(configuration);
            return configuration;
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
        public IServiceProvider BuildServiceProvider()
        {
            IsServicesProviderNotBuild();
            return _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        /// <inheritdoc/>
        public IServiceProvider BuildServiceProvider(bool validateScopes)
        {
            IsServicesProviderNotBuild();
            return _serviceProvider = _serviceCollection.BuildServiceProvider(validateScopes);
        }

        /// <inheritdoc/>
        public IServiceProvider BuildServiceProvider(ServiceProviderOptions options)
        {
            IsServicesProviderNotBuild();
            return _serviceProvider = _serviceCollection.BuildServiceProvider(options);
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

        #region IDisposable members
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _serviceProvider?.Dispose();
            }

            _disposed = true;
        }

        /// <inheritdoc/>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_serviceProvider != null)
            {
                await _serviceProvider.DisposeAsync();
            }

            _serviceProvider = null;
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
