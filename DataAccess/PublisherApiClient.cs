using System.Net.Http;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadMangaApp.Models;

namespace ReadMangaApp.DataAccess
{
    public class PublisherApiClient
    {
        private readonly HttpClient _httpClient;

        public PublisherApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Publisher>> GetAllPublisherAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Publisher>>("publisher/all");
                return result ?? new List<Publisher>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw new Exception("Ошибка при получении списка издательств", ex);
            }
        }

        public async Task<Dictionary<int, List<Publisher>>> GetAllPublishersByAllMangaAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<Dictionary<int, List<Publisher>>>("publisher/by-manga");
                return result ?? new Dictionary<int, List<Publisher>>();
            }
            catch (HttpRequestException ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw new Exception("Ошибка при получении списка манги", ex);
            }
        }
    }
}
