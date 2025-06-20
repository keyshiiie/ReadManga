using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ReadMangaApp.Commands;
using ReadMangaApp.Models; // Класс User

public class AuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = password // plain password
        };

        try
        {
            // Отправляем POST с телом JSON
            var response = await _httpClient.PostAsJsonAsync("Auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                // Десериализуем объект User из ответа
                var user = await response.Content.ReadFromJsonAsync<User>();
                return user;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Неверный логин/пароль
                return null;
            }
            else
            {
                // Другие ошибки
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка сервера: {response.StatusCode}, {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Ошибка при подключении к серверу", ex);
        }
    }

    // Класс запроса (можно вынести отдельно)
    private class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
