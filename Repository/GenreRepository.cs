using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using System.Data;

namespace ReadMangaApp.Repository
{
    class GenreRepository
    {
        public static List<Genre> GetAllGenre(DBConnection dBConnection)
        {
            var genres = new List<Genre>();
            string query = @"SELECT * FROM Genre";
            DataTable dataTable = dBConnection.ExecuteReader(query);
            foreach(DataRow row in dataTable.Rows)
            {
                var genre = new Genre(
                (int)row["id_genre_manga"],
                (string)row["name_genre"]
                );
                genres.Add(genre);
            }
            return genres;
        }

        public static Dictionary<int, List<Genre>> GetAllGenresByAllManga(DBConnection dbConnection)
        {
            var genresByManga = new Dictionary<int, List<Genre>>();
            string query = @"
        SELECT gm.id_manga, g.id_genre_manga, g.name_genre
        FROM MangaGenres gm
        JOIN Genre g ON gm.id_genre_manga = g.id_genre_manga";

            DataTable dataTable = dbConnection.ExecuteReader(query);
            foreach (DataRow row in dataTable.Rows)
            {
                int mangaId = (int)row["id_manga"];
                var genre = new Genre(
                    (int)row["id_genre_manga"],
                    (string)row["name_genre"]
                );
                if (!genresByManga.ContainsKey(mangaId))
                    genresByManga[mangaId] = new List<Genre>();
                genresByManga[mangaId].Add(genre);
            }
            return genresByManga;
        }


    }
}
