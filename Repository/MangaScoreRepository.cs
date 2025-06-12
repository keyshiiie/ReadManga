using ReadMangaApp.Models;
using ControlzEx.Standard;
using Npgsql;
using ReadMangaApp.DataAccess;
using System.Data;
using System.Data.Common;

namespace ReadMangaApp.Repository
{
    class MangaScoreRepository
    {
        // получение средней оценки для всей манги
        public static Dictionary<int, decimal> GetAllAverageScores(DBConnection dbConnection)
        {
            var scoresByManga = new Dictionary<int, decimal>();
            string query = @"
            SELECT id_manga, AVG(score) as average_score
            FROM Manga_scores
            GROUP BY id_manga";

            DataTable dataTable = dbConnection.ExecuteReader(query);
            foreach (DataRow row in dataTable.Rows)
            {
                int mangaId = (int)row["id_manga"];
                decimal averageScore = row["average_score"] == DBNull.Value ? 0 : Convert.ToDecimal(row["average_score"]);
                scoresByManga[mangaId] = averageScore;
            }
            return scoresByManga;
        }

        // добавление или обновление оценки для выбранной манги
        public static string? UpdateScore(DBConnection dBConnection, int idUser, int idManga, decimal score)
        {
            string query = @"SELECT add_manga_score(@IdUser, @IdManga, @Score)";
            var parameters = new[]
            {
                new NpgsqlParameter("@IdUser", idUser),
                new NpgsqlParameter("@IdManga", idManga),
                new NpgsqlParameter("@Score", score)
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

