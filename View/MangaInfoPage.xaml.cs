using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using ReadMangaApp.ViewModels;
using System.Data.Common;
using System.Windows.Controls;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для MangaInfoPage.xaml
    /// </summary>
    public partial class MangaInfoPage : Page
    {
        public MangaInfoPage(INavigationService mainNavigationService, Manga selectedManga)
        {
            InitializeComponent();
            var viewModel = new MangaInfoPageVM(mainNavigationService, selectedManga);
            DataContext = viewModel;
        }
    }
}