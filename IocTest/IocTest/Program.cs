using System;
using Funq.Fast;
using IocTest.Core;

namespace IocTest
{
    class Program
    {
        static void Main()
        {
            var app = new App();
            app.RegisterDependencies();

            var weatherStation = DependencyInjection.Resolve<IWeatherStation>();
            weatherStation.Conditions = new[]
            {
                new WeatherCondition("HOT",  "Yellow"), 
                new WeatherCondition("COLD", "Blue"),
                new WeatherCondition("STORM","DarkGray"),
                new WeatherCondition("SNOW", "White"), 
                new WeatherCondition("WINDY","Gray")
            };
            weatherStation.DisplayReport();

            Console.ReadKey();
        }
    }
}
