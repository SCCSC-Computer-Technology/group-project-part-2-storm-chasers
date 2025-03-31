using Microsoft.AspNetCore.Mvc;
using StormChasersGroupProject2.Models;

namespace StormChasersGroupProject2.Controllers
{
    public class WeatherController : Controller
    {
        //here we will retrieve/fetch the weather data
        public IActionResult Index()
        {
            //adding an example, just until we have the live data attached
            var weather = new WeatherModel
            {
                City = "Spartanburg",
                Temperature = 60.5,
                HumidityPercent = 94,
                WindSpeed = 7,
                PrecipType = "Rain",
                PrecipPercent = 17,
                WeatherIcon = "rainyicon.png"

            };
            return View(weather);
        }
    }
}
