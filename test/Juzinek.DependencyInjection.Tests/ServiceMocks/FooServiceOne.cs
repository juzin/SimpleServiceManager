namespace Juzinek.DependencyInjection.Tests.ServiceMocks
{
    public class FooServiceOne : IFooServiceOne
    {
        public FooServiceOne()
        {
        }

        public int GetTen()
        {
            return 10;
        }
    }
}
