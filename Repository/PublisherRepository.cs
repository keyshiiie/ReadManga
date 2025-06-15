using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using System.Data;

namespace ReadMangaApp.Repository
{
    internal class PublisherRepository
    {
        // получение списка издательсв для всей манги
        public static Dictionary<int, List<Publisher>> GetAllPublishersByAllManga(DBConnection dbConnection)
        {
            var publishersByManga = new Dictionary<int, List<Publisher>>();
            string query = @"
            SELECT mp.id_manga, p.id_publisher_manga, p.name_publisher
            FROM MangaPublishers mp
            JOIN Publisher p ON mp.id_publisher_manga = p.id_publisher_manga";
            try
            {
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
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка издательств для манги:", ex);
            }
            return publishersByManga;
        }
        // получения списка издательств для сортировки
        public static List<Publisher> GetAllPublisher(DBConnection dBConnection)
        {
            var publishers = new List<Publisher>();
            string query = @"SELECT * FROM Publisher";
            try
            {
                DataTable dataTable = dBConnection.ExecuteReader(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    var publisher = new Publisher(
                    (int)row["id_publisher_manga"],
                    (string)row["name_publisher"]
                    );
                    publishers.Add(publisher);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка издательств:", ex);
            }
            return publishers;
        }
    }
}
