using Npgsql;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using System;
using System.Data;

namespace ReadMangaApp.Repository
{
    class TegRepository
    {
        public static List<Teg> GetAllTegs(DBConnection dBConnection)
        {
            var tegs = new List<Teg>();
            string query = @"SELECT * FROM Teg";
            DataTable dataTable = dBConnection.ExecuteReader(query);
            foreach (DataRow row in dataTable.Rows) 
            {
                var teg = new Teg(
                        (int)row["id_teg_manga"],
                        (string)row["name_teg"]
                        );
                tegs.Add(teg);
            }
            return tegs;
        }

        public static Dictionary<int, List<Teg>> GetAllTegsByAllManga(DBConnection dbConnection)
        {
            var tegsByManga = new Dictionary<int, List<Teg>>();
            string query = @"
        SELECT tm.id_manga, t.id_teg_manga, t.name_teg
        FROM MangaTegs tm
        JOIN Teg t ON tm.id_teg_manga = t.id_teg_manga";

            DataTable dataTable = dbConnection.ExecuteReader(query);
            foreach (DataRow row in dataTable.Rows)
            {
                int mangaId = (int)row["id_manga"];
                var teg = new Teg(
                    (int)row["id_teg_manga"],
                    (string)row["name_teg"]
                );
                if (!tegsByManga.ContainsKey(mangaId))
                    tegsByManga[mangaId] = new List<Teg>();
                tegsByManga[mangaId].Add(teg);
            }
            return tegsByManga;
        }


    }
}
