using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StormChasersGroupProject2.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        // Index action to fetch weather data from WeatherAPI.com (GET)
        public async Task<IActionResult> Index() // Default city is Spartanburg
        {
            string city = "Spartanburg";
            // Define the API endpoint and the request URL for current weather
            var currentWeatherUrl = $"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}";
            var currentResponse = await _httpClient.GetStringAsync(currentWeatherUrl);
            var currentWeatherData = JsonConvert.DeserializeObject<dynamic>(currentResponse);

            // Create a WeatherModel instance for the current weather
            var weather = new WeatherModel
            {
                City = currentWeatherData.location.name,
                Temperature = currentWeatherData.current.temp_f,
                HumidityPercent = currentWeatherData.current.humidity,
                WindSpeed = currentWeatherData.current.wind_mph,
                PrecipType = currentWeatherData.current.condition.text, // Rain, Sunny, etc.
                PrecipPercent = currentWeatherData.current.precip_in,
                WeatherIcon = currentWeatherData.current.condition.icon
            };

            // Define the API endpoint and the request URL for the 7-day forecast
            var forecastUrl = $"https://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={city}&days=7";
            var forecastResponse = await _httpClient.GetStringAsync(forecastUrl);
            var forecastData = JsonConvert.DeserializeObject<dynamic>(forecastResponse);

            // Create a list of WeatherForecastModel to hold the forecast data
            var forecast = new List<WeatherForecastModel>();
            foreach (var day in forecastData.forecast.forecastday)
            {
                forecast.Add(new WeatherForecastModel
                {
                    Date = day.date,
                    MaxTemp = day.day.maxtemp_f,
                    MinTemp = day.day.mintemp_f,
                    Condition = day.day.condition.text,
                    Icon = day.day.condition.icon,
                    PrecipPercent = day.day.daily_chance_of_rain
                });
            }

            // Create a WeatherViewModel to pass current weather and forecast to the view
            var viewModel = new WeatherViewModel
            {
                CurrentWeather = weather,
                Forecast = forecast
            };

            return View(viewModel);
        }

        // POST action for city search (POST method should accept the city name)
        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                city = "Spartanburg";
            }

            // Get current weather
            var currentWeatherUrl = $"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}";
            var currentResponse = await _httpClient.GetStringAsync(currentWeatherUrl);
            var currentWeatherData = JsonConvert.DeserializeObject<dynamic>(currentResponse);

            var weather = new WeatherModel
            {
                City = currentWeatherData.location.name,
                Temperature = currentWeatherData.current.temp_f,
                HumidityPercent = currentWeatherData.current.humidity,
                WindSpeed = currentWeatherData.current.wind_mph,
                PrecipType = currentWeatherData.current.condition.text,
                PrecipPercent = currentWeatherData.current.precip_in,
                WeatherIcon = currentWeatherData.current.condition.icon
            };

            // Get 7-day forecast
            var forecastUrl = $"https://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={city}&days=7";
            var forecastResponse = await _httpClient.GetStringAsync(forecastUrl);
            var forecastData = JsonConvert.DeserializeObject<dynamic>(forecastResponse);

            var forecast = new List<WeatherForecastModel>();
            foreach (var day in forecastData.forecast.forecastday)
            {
                forecast.Add(new WeatherForecastModel
                {
                    Date = day.date,
                    MaxTemp = day.day.maxtemp_f,
                    MinTemp = day.day.mintemp_f,
                    Condition = day.day.condition.text,
                    Icon = day.day.condition.icon,
                    PrecipPercent = day.day.daily_chance_of_rain
                });
            }

            var viewModel = new WeatherViewModel
            {
                CurrentWeather = weather,
                Forecast = forecast
            };

            return View(viewModel);
        }

    }
}