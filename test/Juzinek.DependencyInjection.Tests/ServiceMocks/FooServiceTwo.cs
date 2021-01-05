using Microsoft.Extensions.Configuration;
using System;

namespace Juzinek.DependencyInjection.Tests.ServiceMocks
{
    public class FooServiceTwo : IFooServiceTwo
    {
        private readonly IConfiguration _configuration;
        private readonly IFooServiceOne _serviceOne;

        public FooServiceTwo(IConfiguration configuration, IFooServiceOne serviceOne)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (serviceOne is null)
            {
                throw new ArgumentNullException(nameof(serviceOne));
            }
            _configuration = configuration;
            _serviceOne = serviceOne;
        }

        public int GetFifteen()
        {
            return _configuration.GetValue<int>("fooServiceTwo:value");
        }

        public int GetTwentyFive()
        {
            return GetFifteen() + _serviceOne.GetTen();
        }
    }
}
