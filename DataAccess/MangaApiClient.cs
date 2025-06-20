using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ReadMangaApp.Models; // Подключите пространство имен для модели Manga

namespace ReadMangaApp.DataAccess
{
    public class MangaApiClient
    {
        private readonly HttpClient _httpClient;

        public MangaApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        // Получение списка манги
        public async Task<List<Manga>> GetAllMangaAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Manga>>("manga");
                return result ?? new List<Manga>(); // Возвращаем пустой список, если результат null
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
