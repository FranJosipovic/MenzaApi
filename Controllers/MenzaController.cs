using Menza.WebApi.Models;
using Menza.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Menza.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenzaController : ControllerBase
    {
        private readonly MenuRepository _menuRepository;
        private readonly MenzeRespository _menzeRespository;
        public MenzaController(MenzeRespository menzeRespository, MenuRepository menuRepository)
        {
            _menzeRespository = menzeRespository;
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// Gets the menu for a specific menza (restaurant).
        /// </summary>
        /// <param name="menzaId">The identifier of the menza (e.g. "osijek-campus").</param>
        /// <returns>The daily or weekly menu for the specified restaurant.</returns>
        /// <response code="200">Returns the menu for the given menza.</response>
        /// <response code="401">If api key is not passed as header parameter.</response>
        /// <response code="403">If api key is invalid.</response>
        /// <response code="404">If the menza or its menu is not found.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [HttpGet("campus/{menzaId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MealGroup>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCampus(
            [FromHeader(Name = "X-API-KEY")] string apiKey, 
            [FromRoute] string menzaId)
        {
            try
            {
                var menu = await _menuRepository.GetMenuByRestaurantId(menzaId);
                if (menu == null)
                    return NotFound();
                else
                    return Ok(menu);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Finds nearby menza restaurants in Croatia based on user location.
        /// </summary>
        /// <param name="lat">Latitude coordinate of the user (e.g. 45.815).</param>
        /// <param name="lon">Longitude coordinate of the user (e.g. 15.9819).</param>
        /// <param name="city">City name (used if GPS coordinates are unavailable).</param>
        /// <param name="postalCode">Postal code (alternative to coordinates or city).</param>
        /// <returns>A list of nearby menza restaurants matching the given location criteria.</returns>
        /// <response code="200">Returns a list of nearby restaurants.</response>
        /// <response code="400">Returned when no valid location parameters are provided.</response>
        /// <response code="401">If api key is not passed as header parameter.</response>
        /// <response code="403">If api key is invalid.</response>
        /// <response code="500">Returned when an internal server error occurs.</response>
        [HttpGet("nearby")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Restaurant>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNearby(
            [FromHeader(Name = "X-API-KEY")] string apiKey,
            [FromQuery] double? lat,
            [FromQuery] double? lon,
            [FromQuery] string? city,
            [FromQuery] int? postalCode)
        {
            try
            {
                if (lat.HasValue && lon.HasValue)
                {
                    var restaurants = _menzeRespository.GetRestaurantsByLongAndLat(lat.Value, lon.Value);
                    return Ok(restaurants);
                }

                if (!string.IsNullOrWhiteSpace(city))
                {
                    var restaurants = _menzeRespository.GetRestaurantsByCityAsync(city);
                    return Ok(restaurants);
                }

                if (postalCode is not null)
                {
                    var restaurants = _menzeRespository.GetRestaurantsByPostalCodeAsync(postalCode.Value);
                    return Ok(restaurants);
                }

                return BadRequest("Missing location parameters. Provide lat/lon or city or postalCode.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
