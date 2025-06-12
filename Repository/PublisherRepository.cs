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
        public static List<Publisher> GetAllPublisherByMangaId(DBConnection dbConnection, int idManga)
        {
            var publishers = new List<Publisher>();
            string query = @"SELECT p.name_publisher, pm.id_publisher_manga FROM Publisher p JOIN MangaPublishers pm ON p.id_publisher_manga = pm.id_publisher_manga WHERE pm.id_manga = @IdManga";
            var parameters = new[]
            {
                new NpgsqlParameter("IdManga", idManga)
            };
            DataTable dataTable = dbConnection.ExecuteReader(query,parameters);
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
