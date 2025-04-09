
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StormChasersGroupProject2.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace StormChasersGroupProject2.Controllers
{
    public class WeatherController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "7dd351856e3c4ba9ba0212939250604"; //api key from weatherapi.com

        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Index action to fetch weather data from WeatherAPI.com
        public async Task<IActionResult> Index()
        {
            string city = "Spartanburg";
            // Define the API endpoint and the request URL
            var url = $"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}";

            // Make the HTTP request and retrieve the weather data as a string
            var response = await _httpClient.GetStringAsync(url);

            // Deserialize the JSON response into a dynamic object
            var weatherData = JsonConvert.DeserializeObject<dynamic>(response);

            // Create a WeatherModel instance and populate it with data from the API response
            var weather = new WeatherModel
            {
                City = weatherData.location.name,
                Temperature = weatherData.current.temp_f,
                HumidityPercent = weatherData.current.humidity,
                WindSpeed = weatherData.current.wind_mph,
                PrecipType = weatherData.current.condition.text, // Rain, Sunny, etc.
                PrecipPercent = weatherData.current.precip_in,
                WeatherIcon = weatherData.current.condition.icon
            };

            // Pass the populated WeatherModel to the view
            return View(weather);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            if (city == null) city = "Spartanburg";

            // Define the API endpoint and the request URL
            var url = $"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}";

            // Make the HTTP request and retrieve the weather data as a string
            var response = await _httpClient.GetStringAsync(url);

            // Deserialize the JSON response into a dynamic object
            var weatherData = JsonConvert.DeserializeObject<dynamic>(response);

            // Create a WeatherModel instance and populate it with data from the API response
            var weather = new WeatherModel
            {
                City = weatherData.location.name,
                Temperature = weatherData.current.temp_f,
                HumidityPercent = weatherData.current.humidity,
                WindSpeed = weatherData.current.wind_mph,
                PrecipType = weatherData.current.condition.text, // Rain, Sunny, etc.
                PrecipPercent = weatherData.current.precip_in,
                WeatherIcon = weatherData.current.condition.icon
            };

            // Pass the populated WeatherModel to the view
            return View(weather);
        }
    }
}

