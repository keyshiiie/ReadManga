using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Configuration;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для RateMangaPage.xaml
    /// </summary>
    public partial class RateMangaPage
    {
        public RateMangaPage(Manga selectedManga)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);
            DataContext = new RateMangaPageVM(dbConnection, selectedManga, this);
        }
    }
}
