using Juzin.DependencyInjection.Tests.ServiceMocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Juzin.DependencyInjection.Tests
{
    [TestClass]
    public class SimpleServiceManagerTest
    {
        [TestMethod]
        public void InitializeTransientServiceWithInterfaceTest()
        {
            //Arrange
            using var serviceManager = new SimpleServiceManager();
            serviceManager
                .AddTransient<IFooServiceOne, FooServiceOne>()
                .BuildServiceProvider();

            //Act
            var serviceOne = serviceManager.GetRequiredService<IFooServiceOne>();
            
            //Assert
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeTransientServiceTest()
        {
            using var serviceManager = new SimpleServiceManager();
            serviceManager
                .AddTransient<FooServiceOne>()
                .BuildServiceProvider();

            var serviceOne = serviceManager.GetRequiredService<FooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeScopedServiceWithInterfaceTest()
        {
            //Arrange
            using var serviceManager = new SimpleServiceManager();
            serviceManager
                .AddScoped<IFooServiceOne, FooServiceOne>()
                .BuildServiceProvider();

            //Act
            var serviceOne = serviceManager.GetRequiredService<IFooServiceOne>();

            //Assert
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeScopedServiceTest()
        {
            using var serviceManager = new SimpleServiceManager();
            serviceManager
                .AddScoped<FooServiceOne>()
                .BuildServiceProvider();

            var serviceOne = serviceManager.GetRequiredService<FooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeSingletonServiceWithInterfaceTest()
        {
            using var serviceManager = new SimpleServiceManager();
            serviceManager
                .AddSingleton<IFooServiceOne, FooServiceOne>()
                .BuildServiceProvider();

            var serviceOne = serviceManager.GetRequiredService<IFooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeSingletonServiceWithSpecificInstanceTest()
        {
            using var serviceManager = new SimpleServiceManager();
            serviceManager
                .AddSingleton(new FooServiceOne())
                .BuildServiceProvider();

            var serviceOne = serviceManager.GetRequiredService<FooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeSingletonServiceTest()
        {
            using var serviceManager = new SimpleServiceManager();
            serviceManager
                .AddSingleton<FooServiceOne>()
                .BuildServiceProvider();

            var serviceOne = serviceManager.GetRequiredService<FooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void ConfigurationAndServiceInitializationSuccessfulTest()
        {
            using var serviceManager = new SimpleServiceManager();
            serviceManager
                .Configure(c =>
                {
                    c.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                });
            serviceManager.AddTransient<IFooServiceOne, FooServiceOne>()
                .ConfigureServices(s => 
                {
                    s.AddTransient<IFooServiceTwo, FooServiceTwo>();
                })
                .BuildServiceProvider();

            var serviceTwo = serviceManager.GetRequiredService<IFooServiceTwo>();
            Assert.IsNotNull(serviceTwo);
            Assert.AreEqual(15, serviceTwo.GetFifteen());
            Assert.AreEqual(25, serviceTwo.GetTwentyFive());
        }

        [TestMethod]
        public void GetServiceForNotConfiguredServiceReturnsNullTest()
        {
            var serviceManager = new SimpleServiceManager();
            serviceManager.BuildServiceProvider();

            var serviceTwo = serviceManager.GetService<IFooServiceTwo>();
            Assert.IsNull(serviceTwo);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void GetRequiredServiceForNotConfiguredServiceReturnsNullTest()
        {
            using var serviceManager = new SimpleServiceManager();
            serviceManager.BuildServiceProvider();

            _ = serviceManager.GetRequiredService<IFooServiceTwo>();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void GetRequiredServiceBeforeContainerBuiltExpectedExceptionTest()
        {
            using var serviceManager = new SimpleServiceManager();
            _ = serviceManager
                .GetRequiredService<IFooServiceOne>();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void GetServiceBeforeContainerBuiltExpectedExceptionTest()
        {
            using var serviceManager = new SimpleServiceManager();
            _ = serviceManager
                .GetService<IFooServiceOne>();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ConfigureAfterContainerBuiltExpectedExceptionTest()
        {
            using var serviceManager = new SimpleServiceManager();
            serviceManager.BuildServiceProvider();

            _ = serviceManager.Configure(c =>
            {
                c.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            });
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ConfigureServicesAfterContainerBuiltExpectedExceptionTest()
        {
            using var serviceManager = new SimpleServiceManager();
            serviceManager.BuildServiceProvider();

            _ = serviceManager.ConfigureServices(c =>
            {
                c.AddTransient<IFooServiceOne, FooServiceOne>();
            });
        }
    }
}
