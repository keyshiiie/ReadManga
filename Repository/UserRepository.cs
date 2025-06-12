using ReadMangaApp.Models;
using Npgsql;
using ReadMangaApp.DataAccess;
using System.Data;

namespace ReadMangaApp.Repository
{
    internal class UserRepository
    {
        public static List<User> AuthorizationUser(DBConnection dbConnection, string username, string passwordHash)
        {
            var users = new List<User>();
            string query = @"SELECT * FROM Users WHERE username = @username AND password_hash = @passwordHash";
            var parameters = new[]
            {
            new NpgsqlParameter("username", username),
            new NpgsqlParameter("passwordHash", passwordHash)
        };
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
            return users;
        }

        // Метод для регистрации нового пользователя
        public static bool RegisterUser(DBConnection dbConnection, string username, string passwordHash, string email)
        {
            // Проверяем, существует ли уже пользователь с таким именем или электронной почтой
            string checkQuery = @"SELECT COUNT(*) FROM Users WHERE username = @username OR email = @email";
            var checkParameters = new[]
            {
                new NpgsqlParameter("username", username),
                new NpgsqlParameter("email", email)
            };

            int userCount = Convert.ToInt32(dbConnection.ExecuteScalar(checkQuery, checkParameters));
            if (userCount > 0)
            {
                // Пользователь с таким именем или электронной почтой уже существует
                return false;
            }

            // Если нет, добавляем нового пользователя
            string insertQuery = @"INSERT INTO Users (username, password_hash, email) VALUES (@username, @passwordHash, @email)";
            var insertParameters = new[]
            {
                new NpgsqlParameter("username", username),
                new NpgsqlParameter("passwordHash", passwordHash),
                new NpgsqlParameter("email", email)
            };

            // Выполняем запрос вставки
            dbConnection.ExecuteScalar(insertQuery, insertParameters);
            return true; // Успешно зарегистрирован
        }

        public static User? GetUserByUsername(DBConnection dbConnection, string username)
        {
            string query = @"SELECT * FROM Users WHERE username = @username";
            var parameters = new[]
            {
        new NpgsqlParameter("username", username)
    };

            DataTable dataTable = dbConnection.ExecuteReader(query, parameters);

            // Проверяем, если пользователь найден
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0]; // Берем первую строку
                var user = new User(
                    (int)row["id_user"],
                    (string)row["username"],
                    (string)row["password_hash"],
                    (string)row["email"],
                    (DateTime)row["created_at"]
                );
                return user; // Возвращаем найденного пользователя
            }

            return null; // Если пользователь не найден, возвращаем null
        }

    }
}
