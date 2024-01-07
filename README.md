# SimpleServiceManager
Simple wrapper over Microsoft.Extensions.DependencyInjection combining ServiceCollection, ServiceProvider and ConfigurationBuilder. It's useful for application types which does not support DI out of the box (console applications, unit tests, ...). Supports projects targeting .NET8 framework.

## SDK
.NET8 https://dotnet.microsoft.com/en-us/download/visual-studio-sdks

# Getting Started

## Usage
- Service provider can be build only once by calling BuildServiceProvider() method, for direct access use ServiceCollection property
- Implements IDisposable and IAsyncDisposable interfaces for disposing of underlying service provider

```cs
using var serviceManager = new SimpleServiceManager();
serviceManager
    .AddTransient<IFooServiceOne, FooServiceOne>()
    .AddSingleton<IFooServiceTwo, FooServiceTwo>()
    .BuildServiceProvider(); //Builds service provider, after calling this method no other configuration is allowed

//Get service from container with injected dependencies
var serviceOne = serviceManager.GetRequiredService<IFooServiceOne>(); //Gets service, if service does not exist throws exception
var serviceTwo = serviceManager.GetService<IFooServiceTwo>(); //Gets service, if service does not exist returns null
```
### Configure services
```cs
//Add appsettings.json to cofiguration builds and returns IConfigurationRoot
serviceManager
    .Configure(c => 
    {
        c.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
    });

//Convenient access to IServiceCollection, can be used with existing IServiceCollection extensions
serviceManager
    .ConfigureServices(s => 
    {
        s.AddHttpClient();
    });

//Directly exposed IServiceCollection
serviceManager.ServiceCollection   
```

## Tests
```bash
dotnet test
```