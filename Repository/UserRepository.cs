using ReadMangaApp.Models;
using Npgsql;
using ReadMangaApp.DataAccess;
using System.Data;

namespace ReadMangaApp.Repository
{
    internal class UserRepository
    {
        // авторизация пользователя (получение данных пользователя по никнейму и паролю)
        public static List<User> AuthorizationUser(DBConnection dbConnection, string username, string passwordHash)
        {
            var users = new List<User>();
            string query = @"SELECT * FROM Users WHERE username = @username AND password_hash = @passwordHash";
            var parameters = new[]
            {
                new NpgsqlParameter(nameof(username), username),
                new NpgsqlParameter(nameof(passwordHash), passwordHash)
            };
            try
            {
                DataTable dataTable = dbConnection.ExecuteReader(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    var user = new User(
                        (int)row["id_user"],
                        (string)row["username"],
                        (string)row["password_hash"],
                        (string)row["email"],
                        (DateTime)row["created_at"]
                    );
                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка страниц для главы:", ex);
            }
            return users;
        }
    }
}
