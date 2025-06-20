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
    public class ChapterApiClient
    {
        private readonly HttpClient _httpClient;
        public ChapterApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Chapter>> GetAllChaptersAsync(int mangaId)
        {
            try
            {
                var url = $"chapter/manga/{mangaId}";
                var result = await _httpClient.GetFromJsonAsync<List<Chapter>>(url);
                return result ?? new List<Chapter>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                // Возвращаем пустой список, чтобы не ломать UI
                return new List<Chapter>();
            }
        }
    }
}
