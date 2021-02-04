using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Juzin.DependencyInjection
{
    /// <summary>
    /// Simple service manage interface
    /// </summary>
    public interface ISimpleServiceManager
    {
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
        /// Action delegate for configuring IConfigurationBuilder
        /// </summary>
        /// <param name="configureBuilderAction">IConfigurationBuilder action</param>
        /// <returns>Builded <see cref="IConfigurationRoot"/></returns>
        IConfigurationRoot Configure(Action<IConfigurationBuilder> configureBuilderAction);

        /// <summary>
        /// Convenient service configuration using eg. existing IServiceCollection extensions
        /// </summary>
        /// <param name="serviceCollectionAction">IServiceCollection action</param>
        /// <returns></returns>
        ISimpleServiceManager ConfigureServices(Action<IServiceCollection> serviceCollectionAction);

        /// <summary>
        /// Builds service provider. After building its possible to consume services
        /// </summary>
        void BuildServiceProvider();

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