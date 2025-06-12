using Npgsql;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Repository
{
    internal class PagesRepository
    {
        public static List<MangaPage> GetAllPages(DBConnection dbconnection, int idChapter)
        {
            var pages = new List<MangaPage>();
            string query = @"SELECT p.id_page, p.id_chapter, p.page_number, p.page_content_url
                     FROM Page p 
                     WHERE p.id_chapter = @IdChapter
                     ORDER BY p.page_number"; // Добавлена сортировка по page_number

            // Создаем параметр для запроса
            var parameters = new[]
            {
                new NpgsqlParameter("@IdChapter", idChapter)
            };

            // Используем ExecuteReader с параметрами
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
            return pages;
        }


    }
}
