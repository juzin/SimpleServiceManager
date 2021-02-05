# SimpleServiceManager
Simple wrapper over Microsoft.Extensions.DependencyInjection combining ServiceCollection, ServiceProvider and ConfigurationBuilder. It's useful for application types which does not support DI out of the box.
# Usage 
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
# Configuration
```cs
//Add appsettings.json to cofiguration builds and returns IConfigurationRoot
serviceManager
    .Configure(c => 
    {
        c.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
    });

//Convenient direct access to IServiceCollection, can be used with existing IServiceCollection extensions
serviceManager
    .ConfigureServices(s => 
    {
        s.AddHttpClient();
    });
```
## 