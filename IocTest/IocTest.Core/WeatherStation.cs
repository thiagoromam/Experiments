using System;

namespace IocTest.Core
{
    internal class WeatherStation : IWeatherStation
    {
        private readonly IDisplayService _displayService;
        private readonly Random _random;

        public WeatherStation(IDisplayService displayService)
        {
            _displayService = displayService;
            _random = new Random();
        }

        public WeatherCondition[] Conditions { get; set; }

        public void DisplayReport()
        {
            var condition = Conditions[_random.Next(0, Conditions.Length)];
            _displayService.SetColor(condition.Color);
            _displayService.Write(string.Format("{0} @ {1:g}", condition.Name, DateTime.Now));
            _displayService.ResetColor();
        }
    }
}