using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Windows.Controls;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для MangaInfoPage.xaml
    /// </summary>
    public partial class MangaInfoPage : Page
    {
        public MangaInfoPage(Manga selectedManga, IEnumerable<Genre> genres, IEnumerable<Teg> tegs, DBConnection dbConnection)
        {
            InitializeComponent();
            var viewModel = new MangaInfoPageVM(dbConnection, selectedManga, genres, tegs);
            DataContext = viewModel;
        }
    }
}
