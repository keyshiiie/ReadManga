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
    public class GenreApiClient
    {
        private readonly HttpClient _httpClient;
        public GenreApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Genre>>("genre/all");
                return result ?? new List<Genre>(); // Возвращаем пустой список, если результат null
            }
            catch (HttpRequestException ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw new Exception("Ошибка при получении списка манги", ex);
            }
        }

        public async Task<Dictionary<int, List<Genre>>> GetAllGenresByAllMangaAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<Dictionary<int, List<Genre>>>("genre/by-manga");
                return result ?? new Dictionary<int, List<Genre>>();
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
