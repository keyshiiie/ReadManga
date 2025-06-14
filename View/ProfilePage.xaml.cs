using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Configuration;
using System.Windows.Controls;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage(DBConnection dBConnection)
        {
            InitializeComponent();
            var viewModel = new ProfilePageVM(dBConnection);
            DataContext = viewModel;
        }
    }
}
