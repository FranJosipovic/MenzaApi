using Menza.WebApi.Models.enums;
using Menza.WebApi.Services;

namespace Menza.WebApi.Models
{
    public class Meal
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal StudentPrice { get; set; }
        public List<Alergen> Alergens { get; set; } = [];
        public NutrientInfo? NutrientInfo { get; set; }
    }

    public class MealGroup
    {
        public string Name { get; set; } = string.Empty;
        public List<Meal> Meals { get; set; } = [];
    }
}
