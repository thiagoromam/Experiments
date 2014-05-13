using Funq;
using Funq.Fast;
using IocTest.Core;

namespace IocTest
{
    public class App : Core.App
    {
        protected override IRegistration<IDisplayService> RegisterDisplayService()
        {
            return DependencyInjection.Register<IDisplayService>(c => new DisplayService());
        }
    }
}