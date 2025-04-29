using StormChasersGroupProject2.Models;

public class FavoriteCitiesViewModel
{
    public Dictionary<string, WeatherModel> WeatherPerCity { get; set; } = new();
}
