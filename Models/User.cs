namespace ReadMangaApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Пустой конструктор нужен для десериализации
        public User() { }

        // Ваш существующий конструктор можно оставить
        public User(int id, string username, string passwordHash, string email, DateTime createdAt)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            CreatedAt = createdAt;
        }
    }
}
