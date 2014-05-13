using Funq;
using Funq.Fast;

namespace IocTest.Core
{
    public abstract class App
    {
        public void RegisterDependencies()
        {
            RegisterDisplayService();
            RegisterWeatherStation();
        }

        private static void RegisterWeatherStation()
        {
            DependencyInjection.Register<IWeatherStation>(c => new WeatherStation(c.Resolve<IDisplayService>()));
        }

        protected abstract IRegistration<IDisplayService> RegisterDisplayService();
    }
}
