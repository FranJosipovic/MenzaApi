using Menza.WebApi.Models.OsijekMenze;
using System.Text;
using System.Text.Json;

namespace Menza.WebApi.Services
{
    public class OsijekMenzeReader
    {
        private readonly HttpClient _httpClient;

        public OsijekMenzeReader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<GrupaJela>?> GetCampusAsync()
        {
            var endpoint = "/upload/televizor_434.json";
            return await Get(endpoint);
        }

        public async Task<List<GrupaJela>?> GetIstarskaAsync()
        {
            var endpoint = "/upload/televizor_431.json";
            return await Get(endpoint);
        }

        private async Task<List<GrupaJela>?> Get(string endpoint)
        {
            var res = await _httpClient.GetAsync(endpoint);
            if (!res.IsSuccessStatusCode)
                return null;

            var bytes = await res.Content.ReadAsByteArrayAsync();

            var charset = res.Content.Headers.ContentType?.CharSet;

            var encoding = Encoding.GetEncoding("windows-1250");

            var json = encoding.GetString(bytes);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<GrupaJela>? grupe = JsonSerializer.Deserialize<List<GrupaJela>>(json, options);
            return grupe;
        }
    }
}
