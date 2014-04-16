namespace IocTest.Core
{
    public class WeatherCondition
    {
        public string Name { get; set; }
        public string Color { get; set; }

        public WeatherCondition(string name, string color)
        {
            Name = name;
            Color = color;
        }
    }
}