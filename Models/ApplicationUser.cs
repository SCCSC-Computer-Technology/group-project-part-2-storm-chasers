using Microsoft.AspNetCore.Identity;

namespace StormChasersGroupProject2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<string> FavoriteCities { get; set; } = new List<string>();

        public void addCity(string city)
        {
            if (!FavoriteCities.Contains(city))
            {
                FavoriteCities.Add(city);
            }
        }
        public void deleteCity(string city)
        {
            FavoriteCities.Remove(city);
        }
    }

}
