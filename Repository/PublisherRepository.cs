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
    internal class PublisherRepository
    {
        public static Dictionary<int, List<Publisher>> GetAllPublishersByAllManga(DBConnection dbConnection)
        {
            var publishersByManga = new Dictionary<int, List<Publisher>>();
            string query = @"
        SELECT mp.id_manga, p.id_publisher_manga, p.name_publisher
        FROM MangaPublishers mp
        JOIN Publisher p ON mp.id_publisher_manga = p.id_publisher_manga";

            DataTable dataTable = dbConnection.ExecuteReader(query);

            foreach (DataRow row in dataTable.Rows)
            {
                int mangaId = (int)row["id_manga"];
                var publisher = new Publisher(
                    (int)row["id_publisher_manga"],
                    (string)row["name_publisher"]
                );

                if (!publishersByManga.ContainsKey(mangaId))
                    publishersByManga[mangaId] = new List<Publisher>();

                publishersByManga[mangaId].Add(publisher);
            }

            return publishersByManga;
        }

        public static List<Publisher> GetAllPublisher(DBConnection dBConnection)
        {
            var publishers = new List<Publisher>();
            string query = @"SELECT * FROM Publisher";
            DataTable dataTable = dBConnection.ExecuteReader(query);
            foreach (DataRow row in dataTable.Rows)
            {
                var publisher = new Publisher(
                (int)row["id_publisher_manga"],
                (string)row["name_publisher"]
                );
                publishers.Add(publisher);
            }
            return publishers;
        }
    }
}
