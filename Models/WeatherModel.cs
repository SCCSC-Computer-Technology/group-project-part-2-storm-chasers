namespace StormChasersGroupProject2.Models
{
    public class WeatherModel
    {
        public string? City { get; set; }
        public double Temperature { get; set; }
        public double HumidityPercent { get; set; }
        public double WindSpeed { get; set; }
        public string? PrecipType { get; set; }
        public double PrecipPercent { get; set; }
        public string? WeatherIcon { get; set; }
        //this is for the weather icon/image for each weather condition
    }
}
