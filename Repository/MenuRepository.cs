using Menza.WebApi.Models;
using Menza.WebApi.Services;

namespace Menza.WebApi.Repository
{
    public class MenuRepository
    {
        private readonly OsijekMenzeReader _osijekMenzeReader;
        private readonly DataNormalizer _dataNormalizer;
        public MenuRepository(OsijekMenzeReader osijekMenzeReader, DataNormalizer dataNormalizer)
        {
            _osijekMenzeReader = osijekMenzeReader;
            _dataNormalizer = dataNormalizer;
        }

        public async Task<List<MealGroup>?> GetMenuByRestaurantId(string restaurantId)
        {
            if(restaurantId == "osijek-istarska")
            {
                var meals = await _osijekMenzeReader.GetIstarskaAsync();
                if (meals == null) return [];
                return _dataNormalizer.NormalizeOsijekData(meals);
            }else if(restaurantId == "osijek-campus")
            {
                var meals = await _osijekMenzeReader.GetCampusAsync();
                if(meals == null) return [];
                return _dataNormalizer.NormalizeOsijekData(meals);
            }else if(restaurantId == "osijek-gaudeamus")
            {
                return [];
            }
            else
            {
                return null;
            }
        }
    }
}
