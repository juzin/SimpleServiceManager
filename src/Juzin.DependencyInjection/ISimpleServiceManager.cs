using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Juzin.DependencyInjection
{
    /// <summary>
    /// Simple service manage interface
    /// </summary>
    public interface ISimpleServiceManager : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Service Collection with registered types
        /// </summary>
        IServiceCollection ServiceCollection { get; }
        
        /// <summary>
        /// Adds a singleton service of the type specified in TService with an implementation type specified in TImplementation to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns></returns>
        ISimpleServiceManager AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// Adds a singleton service of the type specified in TService to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns></returns>
        ISimpleServiceManager AddSingleton<TService>() where TService : class;

        /// <summary>
        ///  Adds a singleton service of the type specified in TService with an instance specified in implementationInstance to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationInstance"></param>
        /// <returns></returns>
        ISimpleServiceManager AddSingleton<TService>(TService implementationInstance) where TService : class;

        /// <summary>
        /// Adds a transient service of the type specified in TService with an implementation type specified in TImplementation to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns></returns>
        ISimpleServiceManager AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        ///  Adds a transient service of the type specified in TService to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns></returns>
        ISimpleServiceManager AddTransient<TService>() where TService : class;

        /// <summary>
        ///  Adds a scoped service of the type specified in TService with an implementation type specified in TImplementation to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns></returns>
        ISimpleServiceManager AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        ///  Adds a scoped service of the type specified in TService to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns></returns>
        ISimpleServiceManager AddScoped<TService>()
            where TService : class;

        /// <summary>
        /// Action delegate for configuring IConfigurationBuilder.
        /// </summary>
        /// <param name="configureBuilderAction">IConfigurationBuilder action</param>
        /// <returns>Created <see cref="IConfigurationRoot"/></returns>
        IConfigurationRoot Configure(Action<IConfigurationBuilder> configureBuilderAction);

        /// <summary>
        /// Convenient service configuration using eg. existing IServiceCollection extensions.
        /// </summary>
        /// <param name="serviceCollectionAction">IServiceCollection action</param>
        /// <returns></returns>
        ISimpleServiceManager ConfigureServices(Action<IServiceCollection> serviceCollectionAction);

        /// <summary>
        /// Builds service provider and returns <see cref="IServiceProvider"/>. After building its possible to consume services.
        /// </summary>
        /// <returns></returns>
        IServiceProvider BuildServiceProvider();

        /// <summary>
        /// Builds service provider and returns <see cref="IServiceProvider"/>. After building its possible to consume services.
        /// </summary>
        /// <param name="validateScopes">true to perform check verifying that scoped services never gets resolved from root provider, otherwise false.</param>
        /// <returns></returns>
        IServiceProvider BuildServiceProvider(bool validateScopes);

        /// <summary>
        /// Builds service provider and returns <see cref="IServiceProvider"/>. After building its possible to consume services.
        /// </summary>
        /// <param name="options">Configures various service provider behaviors.</param>
        /// <returns></returns>
        IServiceProvider BuildServiceProvider(ServiceProviderOptions options);

        /// <summary>
        /// Get service of type T from the System.IServiceProvider.
        /// Throws exception if requested services was not configured.
        /// </summary>
        /// <typeparam name="TService">The type of service object to get.</typeparam>
        /// <returns></returns>
        TService GetRequiredService<TService>() where TService : class;

        /// <summary>
        /// Get service of type T from the System.IServiceProvider. 
        /// Returns null if requested service was not configured.
        /// </summary>
        /// <typeparam name="TService">The type of service object to get.</typeparam>
        /// <returns></returns>
        TService GetService<TService>() where TService : class;
    }
}