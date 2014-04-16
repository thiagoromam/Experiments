using Funq;

namespace IocTest.Core
{
    public abstract class App
    {
        protected App(Container container)
        {
            Container = container;
        }

        internal static Container Container { get; private set; }

        public void RegisterDependencies()
        {
            RegisterDisplayService(Container);
            RegisterWeatherStation();
        }

        private static void RegisterWeatherStation()
        {
            Container.Register<IWeatherStation>(c => new WeatherStation(c.Resolve<IDisplayService>()));
        }

        protected abstract IRegistration<IDisplayService> RegisterDisplayService(Container container);
    }
}
