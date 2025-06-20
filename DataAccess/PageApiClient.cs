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
    public class PageApiClient
    {
        private readonly HttpClient _httpClient;
        public PageApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<MangaPage>> GetAllChapterPagesAsync(int chapterId)
        {
            try
            {
                var url = $"pages/chapter/{chapterId}";
                var result = await _httpClient.GetFromJsonAsync<List<MangaPage>>(url);
                return result ?? new List<MangaPage>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                // Возвращаем пустой список, чтобы не ломать логику приложения
                return new List<MangaPage>();
            }
        }
    }
}