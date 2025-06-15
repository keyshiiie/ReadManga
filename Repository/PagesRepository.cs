using Npgsql;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using System.Data;

namespace ReadMangaApp.Repository
{
    internal class PagesRepository
    {
        // Получение списка страниц для главы
        public static List<MangaPage> GetAllPagesByChapter(DBConnection dbconnection, int chapterId)
        {
            var pages = new List<MangaPage>();
            string query = @"SELECT p.id_page, p.id_chapter, p.page_number, p.page_content_url
                     FROM Page p 
                     WHERE p.id_chapter = @chapterId
                     ORDER BY p.page_number"; // Добавлена сортировка по page_number
            // Создаем параметр для запроса
            var parameters = new[]
            {
                new NpgsqlParameter(nameof(chapterId), chapterId)
            };
            // Используем ExecuteReader с параметрами
            try
            {
                DataTable dataTable = dbconnection.ExecuteReader(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    var page = new MangaPage(
                        (int)row["id_page"],
                        null,
                        (int)row["page_number"],
                        (string)row["page_content_url"]
                    );
                    pages.Add(page);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка страниц для главы:", ex);
            }
            return pages;
        }


    }
}
