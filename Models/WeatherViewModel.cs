namespace StormChasersGroupProject2.Models
{

    public class WeatherViewModel
    {
        public WeatherModel CurrentWeather { get; set; }
        public List<WeatherForecastModel> Forecast { get; set; }
    }
}