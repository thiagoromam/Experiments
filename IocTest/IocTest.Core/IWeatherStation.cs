namespace IocTest.Core
{
    public interface IWeatherStation
    {
        WeatherCondition[] Conditions { get; set; }
        void DisplayReport();
    }
}