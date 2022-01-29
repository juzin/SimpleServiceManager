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
        private ISimpleServiceManager _serviceManager;

        [TestInitialize]
        public void TestInitialize()
        {
            _serviceManager = new SimpleServiceManager();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _serviceManager.Dispose();
        }
        
        [TestMethod]
        public void InitializeTransientServiceWithInterfaceTest()
        {
            //Arrange
            _serviceManager
                .AddTransient<IFooServiceOne, FooServiceOne>()
                .BuildServiceProvider();

            //Act
            var serviceOne = _serviceManager.GetRequiredService<IFooServiceOne>();
            
            //Assert
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeTransientServiceTest()
        {
            _serviceManager
                .AddTransient<FooServiceOne>()
                .BuildServiceProvider();

            var serviceOne = _serviceManager.GetRequiredService<FooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeScopedServiceWithInterfaceTest()
        {
            //Arrange
            _serviceManager
                .AddScoped<IFooServiceOne, FooServiceOne>()
                .BuildServiceProvider();

            //Act
            var serviceOne = _serviceManager.GetRequiredService<IFooServiceOne>();

            //Assert
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeScopedServiceTest()
        {
            _serviceManager
                .AddScoped<FooServiceOne>()
                .BuildServiceProvider();

            var serviceOne = _serviceManager.GetRequiredService<FooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeSingletonServiceWithInterfaceTest()
        {
            _serviceManager
                .AddSingleton<IFooServiceOne, FooServiceOne>()
                .BuildServiceProvider();

            var serviceOne = _serviceManager.GetRequiredService<IFooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeSingletonServiceWithSpecificInstanceTest()
        {
            _serviceManager
                .AddSingleton(new FooServiceOne())
                .BuildServiceProvider();

            var serviceOne = _serviceManager.GetRequiredService<FooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void InitializeSingletonServiceTest()
        {
            _serviceManager
                .AddSingleton<FooServiceOne>()
                .BuildServiceProvider();

            var serviceOne = _serviceManager.GetRequiredService<FooServiceOne>();
            Assert.IsNotNull(serviceOne);
            Assert.AreEqual(10, serviceOne.GetTen());
        }

        [TestMethod]
        public void ConfigurationAndServiceInitializationSuccessfulTest()
        {
            _serviceManager
                .Configure(c =>
                {
                    c.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                });
            _serviceManager.AddTransient<IFooServiceOne, FooServiceOne>()
                .ConfigureServices(s => 
                {
                    s.AddTransient<IFooServiceTwo, FooServiceTwo>();
                })
                .BuildServiceProvider();

            var serviceTwo = _serviceManager.GetRequiredService<IFooServiceTwo>();
            Assert.IsNotNull(serviceTwo);
            Assert.AreEqual(15, serviceTwo.GetFifteen());
            Assert.AreEqual(25, serviceTwo.GetTwentyFive());
        }

        [TestMethod]
        public void GetServiceForNotConfiguredServiceReturnsNullTest()
        {
            _serviceManager.BuildServiceProvider();

            var serviceTwo = _serviceManager.GetService<IFooServiceTwo>();
            Assert.IsNull(serviceTwo);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void GetRequiredServiceForNotConfiguredServiceReturnsNullTest()
        {
            _serviceManager.BuildServiceProvider();

            _ = _serviceManager.GetRequiredService<IFooServiceTwo>();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void GetRequiredServiceBeforeContainerBuiltExpectedExceptionTest()
        {
            _ = _serviceManager
                .GetRequiredService<IFooServiceOne>();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void GetServiceBeforeContainerBuiltExpectedExceptionTest()
        {
            _ = _serviceManager
                .GetService<IFooServiceOne>();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ConfigureAfterContainerBuiltExpectedExceptionTest()
        {
            _serviceManager.BuildServiceProvider();

            _ = _serviceManager.Configure(c =>
            {
                c.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            });
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ConfigureServicesAfterContainerBuiltExpectedExceptionTest()
        {
            _serviceManager.BuildServiceProvider();

            _ = _serviceManager.ConfigureServices(c =>
            {
                c.AddTransient<IFooServiceOne, FooServiceOne>();
            });
        }
    }
}
