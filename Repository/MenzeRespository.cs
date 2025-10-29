using System.Text.Json;

namespace Menza.WebApi.Repository
{
    public class MenzeRespository
    {
        private const double SearchRadiusKm = 30.0;
        public List<Restaurant> GetRestaurantsByCityAsync(string city)
        {
            var restaurantrs = GetFromMockFile();
            return restaurantrs.Where(r => r.City == city).ToList();
        }
        public List<Restaurant> GetRestaurantsByPostalCodeAsync(int postalCode)
        {
            var restaurantrs = GetFromMockFile();
            return restaurantrs.Where(r => r.PostalCode == postalCode).ToList();
        }
        public List<Restaurant> GetRestaurantsByLongAndLat(double longitude, double latitude)
        {
            var restaurants = GetFromMockFile();

            return restaurants
                .Where(r => GetDistanceKm(latitude, longitude, r.Latitude, r.Longitude) <= SearchRadiusKm)
                .ToList();
        }

        private List<Restaurant> GetFromMockFile()
        {
            var menzeFile = Path.Combine(AppContext.BaseDirectory, "restorani.json");
            if (!File.Exists(menzeFile))
                throw new FileNotFoundException($"Nutrient data file not found at path: {menzeFile}");

            var menzeJson = File.ReadAllText(menzeFile);

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<Restaurant>>(menzeJson, options) ?? [];
        }

        private static double GetDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusKm = 6371.0;

            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
