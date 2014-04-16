using Funq;
using IocTest.Core;

namespace IocTest
{
    public class App : Core.App
    {
        public static Container Container { get; private set; }

        static App()
        {
            Container = new Container();
        }
        public App() : base(Container)
        {
        }

        protected override IRegistration<IDisplayService> RegisterDisplayService(Container container)
        {
            return container.Register<IDisplayService>(c => new DisplayService());
        }
    }
}