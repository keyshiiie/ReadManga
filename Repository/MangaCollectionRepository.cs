using ReadMangaApp.Models;
using Microsoft.VisualBasic;
using Npgsql;
using ReadMangaApp.DataAccess;
using System.Data;

namespace ReadMangaApp.Repository
{
    class MangaCollectionRepository
    {
        public static Dictionary<int, string> GetAllCollectionByManga(DBConnection dbConnection, int userId)
        {
            var collectionByManga = new Dictionary<int, string>();
            string query = @"SELECT mc.id_manga, c.title
                     FROM MangaCollection mc
                     JOIN Collection c ON mc.id_collection = c.id_collection
                     WHERE mc.id_user = @UserId";

            var parameters = new NpgsqlParameter[]
            {
        new NpgsqlParameter("@UserId", userId)
            };

            DataTable dataTable = dbConnection.ExecuteReader(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                int mangaId = (int)row["id_manga"];
                string collectionTitle = (string)row["title"];
                collectionByManga[mangaId] = collectionTitle;
            }
            return collectionByManga;
        }

        // получение списка коллекций для пользователя
        public static List<MangaCollection> GetAllCollectionsByUser(DBConnection dBConnection, int userId, User user)
        {
            var collections = new List<MangaCollection>();
            string query = @"SELECT id_collection, title FROM Collection WHERE id_user = @IdUser";
            var parameters = new[]
            {
                new NpgsqlParameter("IdUser", userId)
            };
            DataTable dataTable = dBConnection.ExecuteReader(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                var collection = new MangaCollection
                    (
                    (int)row["id_collection"],
                    (string)row["title"],
                    user
                    );
                collections.Add(collection);
            }
            return collections;
        }

        // добавление или обновление коллекции для выбранной манги
        public static string? UpdateMangasCollection(DBConnection dBConnection, int userId, int mangaId, int collectionId)
        {
            string query = @"SELECT add_or_update_manga_in_collection(@IdUser, @IdManga, @IdCollection)";
            var parameters = new[]
            {
                new NpgsqlParameter("@IdUser", userId),
                new NpgsqlParameter("@IdManga", mangaId),
                new NpgsqlParameter("@IdCollection", collectionId)
            };
            try
            {
                var result = dBConnection.ExecuteScalar(query, parameters);
                return result?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка:", ex);
            }
        }
    }
}
