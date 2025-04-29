using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using StormChasersGroupProject2.Models;

namespace StormChasersGroupProject2.Controllers
{
    public class WeatherController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "7dd351856e3c4ba9ba0212939250604"; //api key from weatherapi.com

        public WeatherController(HttpClient httpClient, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //main action to display current weather and forecast
        //set default to spartanburg if no city is entered
        [HttpGet]
        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                city = "Spartanburg";
            }

            //get current weather
            var currentWeatherUrl = $"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}";
            var currentResponse = await _httpClient.GetStringAsync(currentWeatherUrl);
            var currentWeatherData = JsonConvert.DeserializeObject<dynamic>(currentResponse);

            //create a WeatherModel instance and populate it with data from the api response
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

            ViewData["FavoriteCitiesModel"] = await GetFavoriteCitiesWeather();

            //get 7 day forecast
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
            //pass the weather model to the view
            return View(viewModel);
        }

        //handles POST requests for city search
        [HttpPost]
        public IActionResult IndexPost(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                city = "Spartanburg";

            //redirects to get
            return RedirectToAction("Index", new { city });
        }

        // gets weather data for all of the user's favorite cities
        private async Task<FavoriteCitiesViewModel> GetFavoriteCitiesWeather()
        {
            var user = await _userManager.GetUserAsync(User);
            var cities = user?.FavoriteCities ?? new List<string>();

            var model = new FavoriteCitiesViewModel();

            foreach (var city in cities)
            {
                var url = $"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}";
                var response = await _httpClient.GetStringAsync(url);
                var weatherData = JsonConvert.DeserializeObject<dynamic>(response);

                //all of the data we want to use in our model
                var weather = new WeatherModel
                {
                    City = weatherData.location.name,
                    Temperature = weatherData.current.temp_f,
                    HumidityPercent = weatherData.current.humidity,
                    WindSpeed = weatherData.current.wind_mph,
                    PrecipType = weatherData.current.condition.text,
                    PrecipPercent = weatherData.current.precip_in,
                    WeatherIcon = weatherData.current.condition.icon
                };

                model.WeatherPerCity[city] = weather;
            }

            return model;
        }

        //adds a city to favorites
        [HttpPost]
        public async Task<IActionResult> FavoriteCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            user.addCity(city);
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", new { city });
        }

        //removes a city from favorites
        [HttpPost]
        public async Task<IActionResult> RemoveFavoriteCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return BadRequest();

            var user = await _userManager.GetUserAsync(User);

            if (user == null)  return Challenge();

            user.deleteCity(city);
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            var model = await GetFavoriteCitiesWeather();
            return Ok();
        }

       //shows favorites in partial view
        [HttpGet]
        public async Task<IActionResult> FavoritesSidebar()
        {
            var model = await GetFavoriteCitiesWeather();
            return PartialView("_FavoriteCitiesPartial", model);
        }

        //shows city suggestions based on what is entered in the search box
        [HttpGet]
        public async Task<IActionResult> GetCitySuggestions(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<string>());

            var url = $"https://api.weatherapi.com/v1/search.json?key={_apiKey}&q={query}";
            var response = await _httpClient.GetStringAsync(url);
            var suggestions = JsonConvert.DeserializeObject<List<dynamic>>(response);

            var cities = suggestions.Select(s => s.name.ToString()).ToList();
            return Json(cities);
        }
    }
}

