using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для RateMangaPage.xaml
    /// </summary>
    public partial class RateMangaWindow
    {
        public RateMangaWindow(Manga selectedManga, MangaScoreApiClient mangaScoreApiClient)
        {
            InitializeComponent();
            var viewModel = new RateMangaWindowVM(selectedManga, mangaScoreApiClient);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close(); // ← подписка на событие

            DataContext = viewModel;
        }
    }
}