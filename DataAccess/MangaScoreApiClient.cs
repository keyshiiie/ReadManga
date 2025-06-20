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
    public class MangaScoreApiClient
    {
        private readonly HttpClient _httpClient;
        public MangaScoreApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public async Task<Dictionary<int, decimal>> GetAllAverageScoresAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<Dictionary<int, decimal>>("mangascore/averages");
                return result ?? new Dictionary<int, decimal>(); // Возвращаем пустой список, если результат null
            }
            catch (HttpRequestException ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw new Exception("Ошибка при получении списка манги", ex);
            }
        }

        public async Task SubmitScoreAsync(int userId, int mangaId, int score)
        {
            var payload = new
            {
                iduser = userId,
                idmanga = mangaId,
                score = score
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("mangascore/update", payload);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при отправке оценки: {ex.Message}");
                throw;
            }
        }
    }
}
