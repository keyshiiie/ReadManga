using Npgsql;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using System.Data;

namespace ReadMangaApp.Repository
{
    class ChapterRepository
    {
        public static List<Chapter> GetAllChapter(DBConnection dbconnection, int idManga)
        {
            var chapters = new List<Chapter>();
            string query = @"SELECT ch.id_chapter, ch.chapter_title, ch.date_published, ch.number_chapter
                     FROM Chapter ch WHERE ch.id_manga = @IdManga  ORDER BY ch.number_chapter"; // Добавлена сортировка по number_chapter";
            // Создаем параметр для запроса
            var parameters = new[]
            {
                new NpgsqlParameter("@IdManga", idManga)
            };
            // Используем ExecuteReader с параметрами
            DataTable dataTable = dbconnection.ExecuteReader(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                var chapter = new Chapter(
                    (int)row["id_chapter"],
                    null,
                    (string)row["chapter_title"],
                    (DateTime)row["date_published"],
                    (int)row["number_chapter"]
                );
                chapters.Add(chapter);
            }
            return chapters;
        }

    }
}
