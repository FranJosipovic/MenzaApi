using Menza.WebApi.Models;
using Menza.WebApi.Models.enums;
using Menza.WebApi.Models.OsijekMenze;
using System.Text.Json;

namespace Menza.WebApi.Services
{
    public class DataNormalizer
    {
        public List<MealGroup> NormalizeOsijekData(List<GrupaJela> mealGroups)
        {
            var nutrientsFile = Path.Combine(AppContext.BaseDirectory, "nutrients.json");
            var alergensFile = Path.Combine(AppContext.BaseDirectory, "osijek_jela_alergeni.json");

            if (!File.Exists(nutrientsFile) || !File.Exists(alergensFile))
                throw new FileNotFoundException($"Nutrient data file not found at path: {nutrientsFile}");

            var nutrientsJson = File.ReadAllText(nutrientsFile);

            var alergensJson = File.ReadAllText(alergensFile);

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            List<NutrientInfo> nutrientInfo = JsonSerializer.Deserialize<List<NutrientInfo>>(nutrientsJson, options) ?? [];

            List<AlergenInfo> alergensInfo = JsonSerializer.Deserialize<List<AlergenInfo>>(alergensJson, options) ?? [];

            List<MealGroup> mealGroupsNoirmalized = [];

            foreach (var mealGroup in mealGroups)
            {
                List<Meal> newMeals = [];
                foreach (Jelo meal in mealGroup.JelaUGrupi)
                {
                    var ni = nutrientInfo.FirstOrDefault(ni => ni.Sifra == meal.Sifra);

                    var allergenEntry = alergensInfo.FirstOrDefault(a => a.Sifra == meal.Sifra);
                    List<Alergen> allergens;

                    if (allergenEntry?.Alergeni is not null)
                    {
                        allergens = allergenEntry.Alergeni
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(code =>
                            {
                                if (int.TryParse(code.Trim(), out int value) &&
                                    Enum.IsDefined(typeof(Alergen), value))
                                {
                                    return (Alergen)value;
                                }
                                return (Alergen?)null; // invalid code
                            })
                            .Where(a => a.HasValue)
                            .Select(a => a!.Value)
                            .ToList();
                    }else
                    {
                        allergens = [];
                    }

                    Meal newMeal = new()
                    {
                        Code = meal.Sifra,
                        Name = meal.Naziv,
                        StudentPrice = meal.StudCijena,
                        Price = meal.Cijena,
                        NutrientInfo = ni,
                        Alergens = allergens,
                    };
                    newMeals.Add(newMeal);
                }
                MealGroup newMealGroup = new() { Name = mealGroup.NazivGrupe, Meals = newMeals };
                mealGroupsNoirmalized.Add(newMealGroup);
            }

            return mealGroupsNoirmalized;
        }
    }

    public class NutrientInfo
    {
        public string Sifra { get; set; } = string.Empty;
        public double Proteins { get; set; }
        public double Carbs { get; set; }
        public double Sugars { get; set; }
        public double Fat { get; set; }
        public double Calories { get; set; }
    }

    public class AlergenInfo
    {
        public string Sifra { get; set; } = string.Empty;
        public string Alergeni { get; set; } //csv
    }
}
