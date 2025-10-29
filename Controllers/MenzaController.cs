using Menza.WebApi.Models.OsijekMenze;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Menza.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenzaController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MenzaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("/istarska")]
        public async Task<IActionResult> GetIstarska()
        {
            using var client = _httpClientFactory.CreateClient("stucos");

            var endpoint = "/upload/televizor_431.json";

            var res = await client.GetAsync(endpoint);
            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, "Failed to fetch JSON");

            // Read content bytes (raw)
            var bytes = await res.Content.ReadAsByteArrayAsync();

            // Try to detect encoding from headers
            var charset = res.Content.Headers.ContentType?.CharSet;

            // Default to UTF-8 if nothing specified
            var encoding = !string.IsNullOrWhiteSpace(charset)
                ? System.Text.Encoding.GetEncoding(charset)
                : System.Text.Encoding.GetEncoding("ISO-8859-2");

            // Decode manually
            var json = encoding.GetString(bytes);

            // Deserialize JSON safely
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<GrupaJela>? grupe = JsonSerializer.Deserialize<List<GrupaJela>>(json, options);

            if (grupe == null || grupe.Count == 0)
                return NotFound("No menu data found");

            return Ok(grupe);
        }

        [HttpGet("/campus")]
        public async Task<IActionResult> GetCampus()
        {
            using var client = _httpClientFactory.CreateClient("stucos");

            var endpoint = "/upload/televizor_434.json";

            var res = await client.GetAsync(endpoint);
            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, "Failed to fetch JSON");

            // Read content bytes (raw)
            var bytes = await res.Content.ReadAsByteArrayAsync();

            // Try to detect encoding from headers
            var charset = res.Content.Headers.ContentType?.CharSet;

            // Default to UTF-8 if nothing specified
            var encoding = Encoding.GetEncoding("windows-1250");

            // Decode manually
            var json = encoding.GetString(bytes);

            // Deserialize JSON safely
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<GrupaJela>? grupe = JsonSerializer.Deserialize<List<GrupaJela>>(json, options);

            if (grupe == null || grupe.Count == 0)
                return NotFound("No menu data found");

            return Ok(grupe);
        }
    }
}
