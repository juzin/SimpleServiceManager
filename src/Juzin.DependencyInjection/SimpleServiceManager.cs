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

        private readonly IConfigurationBuilder _configurationBuilder;
        private ServiceProvider _serviceProvider;
        private bool _disposed;
        #endregion

        #region Properties

        /// <summary>
        /// Service collections
        /// </summary>
        public IServiceCollection ServiceCollection { get; } = new ServiceCollection();

        #endregion
        
        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="SimpleServiceManager"/> class.
        /// </summary>
        public SimpleServiceManager()
        {
            ServiceCollection.AddSingleton<ISimpleServiceManager>(this);
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
            ServiceCollection.AddSingleton<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddSingleton<TService>()
            where TService : class
        {
            IsServicesProviderNotBuild();
            ServiceCollection.AddSingleton<TService>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddSingleton<TService>(TService implementationInstance)
            where TService : class
        {
            IsServicesProviderNotBuild();
            ServiceCollection.AddSingleton(implementationInstance);
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            IsServicesProviderNotBuild();
            ServiceCollection.AddTransient<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddTransient<TService>()
            where TService : class
        {
            IsServicesProviderNotBuild();
            ServiceCollection.AddTransient<TService>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            IsServicesProviderNotBuild();
            ServiceCollection.AddScoped<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc/>
        public ISimpleServiceManager AddScoped<TService>()
            where TService : class
        {
            IsServicesProviderNotBuild();
            ServiceCollection.AddScoped<TService>();
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
            ServiceCollection.AddSingleton<IConfiguration>(configuration);
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
            serviceCollectionAction.Invoke(ServiceCollection);
            return this;
        }

        /// <inheritdoc/>
        public IServiceProvider BuildServiceProvider()
        {
            IsServicesProviderNotBuild();
            return _serviceProvider = ServiceCollection.BuildServiceProvider();
        }

        /// <inheritdoc/>
        public IServiceProvider BuildServiceProvider(bool validateScopes)
        {
            IsServicesProviderNotBuild();
            return _serviceProvider = ServiceCollection.BuildServiceProvider(validateScopes);
        }

        /// <inheritdoc/>
        public IServiceProvider BuildServiceProvider(ServiceProviderOptions options)
        {
            IsServicesProviderNotBuild();
            return _serviceProvider = ServiceCollection.BuildServiceProvider(options);
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

        /// <summary>
        /// Disposes underlying service provider.
        /// </summary>
        /// <param name="disposing"></param>
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

        /// <summary>
        /// Disposes underlying service provider async.
        /// </summary>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_serviceProvider is not null)
            {
                await _serviceProvider.DisposeAsync();
            }
            _serviceProvider = null;
        }
        #endregion

        #region Private members
        private void IsServicesProviderNotBuild()
        {
            if (_serviceProvider is not null)
            {
                throw new InvalidOperationException("Service provider is already build. You cannot add additional services or setup any configuration.");
            }
        }

        private void IsServiceProviderBuild()
        {
            if (_serviceProvider is null)
            {
                throw new InvalidOperationException("Service provider is not build. Call BuildServiceProvider() before calling get service from container.");
            }
        }
        #endregion
    }
}
