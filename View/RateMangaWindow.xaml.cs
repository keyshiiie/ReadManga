using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Configuration;
using System.Data.Common;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для RateMangaPage.xaml
    /// </summary>
    public partial class RateMangaWindow
    {
        public RateMangaWindow(Manga selectedManga, DBConnection dBConnection)
        {
            InitializeComponent();
            var viewModel = new RateMangaPageVM(dBConnection, selectedManga);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close(); // ← подписка на событие

            DataContext = viewModel;
        }
    }
}
