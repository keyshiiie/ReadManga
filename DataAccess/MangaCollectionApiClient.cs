using Microsoft.VisualBasic;
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
    public class MangaCollectionApiClient
    {
        private readonly HttpClient _httpClient;
        public MangaCollectionApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Dictionary<int, string>> GetCollectionsByMangaForUserAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<Dictionary<int, string>>("MangaCollection/collections-by-manga");
                return result ?? new Dictionary<int, string>(); // Возвращаем пустой список, если результат null
            }
            catch (HttpRequestException ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw new Exception("Ошибка при получении списка коллекций для манги", ex);
            }
        }

        public async Task<List<MangaCollection>> GetAllCollectionsByUserAsync(int userId)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<MangaCollection>>("MangaCollection/all-collections");
                return result ?? new List<MangaCollection>();
            }
            catch (HttpRequestException ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw new Exception("Ошибка при получении списка коллекций пользователя", ex);
            }
        }

        public async Task UpdateMangasCollectionAsync(int mangaId, int collectionId)
        {
            var payload = new
            {
                MangaId = mangaId,
                CollectionId = collectionId
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("MangaCollection/update", payload);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при отправке коллекции: {ex.Message}");
                throw;
            }
        }
    }
}
