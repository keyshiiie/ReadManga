using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using System.Data;

namespace ReadMangaApp.Repository
{
    class MangaRepository
    {
        public static List<Manga> GetAllManga(DBConnection dbConnection)
        {
            var mangas = new List<Manga>();
            string query = @"
        SELECT 
            m.id_manga, 
            m.name_manga, 
            m.date_published, 
            m.cover_url, 
            m.author,
            m.description,
            m.alternative_title,
            sr.id_status_released, 
            sr.release_status_name, 
            st.id_status_translation, 
            st.translation_status_name,
            t.id_type,
            t.type_name
        FROM Manga m
        JOIN StatusReleased sr ON m.id_status_released = sr.id_status_released
        JOIN StatusTranslation st ON m.id_status_translation = st.id_status_translation
        JOIN Type t ON m.id_type = t.id_type";

            DataTable dataTable = dbConnection.ExecuteReader(query);
            foreach (DataRow row in dataTable.Rows)
            {
                var manga = new Manga(
                    (int)row["id_manga"],
                    (string)row["name_manga"],
                    (int)row["date_published"],
                    (string)row["cover_url"],
                    new StatusReleased((int)row["id_status_released"], (string)row["release_status_name"]),
                    new StatusTranslation((int)row["id_status_translation"], (string)row["translation_status_name"]),
                    new TypeManga((int)row["id_type"], (string)row["type_name"]),
                    (string)row["author"],
                    (string)row["description"],
                    (string)row["alternative_title"]
                );
                mangas.Add(manga);
            }
            return mangas;
        }
    }
}
