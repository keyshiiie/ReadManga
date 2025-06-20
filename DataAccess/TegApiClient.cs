using ReadMangaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.DataAccess
{
    public class TegApiClient
    {
        private readonly HttpClient _httpClient;
        public TegApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Teg>> GetAllTegsAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Teg>>("teg/all");
                return result ?? new List<Teg>(); // Возвращаем пустой список, если результат null
            }
            catch (HttpRequestException ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw new Exception("Ошибка при получении списка манги", ex);
            }
        }

        public async Task<Dictionary<int, List<Teg>>> GetAllTegsByAllMangaAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<Dictionary<int, List<Teg>>>("teg/by-manga");
                return result ?? new Dictionary<int, List<Teg>>();
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
