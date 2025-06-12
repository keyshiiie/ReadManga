using AdminPartRM.ViewModels;
using ReadMangaApp.DataAccess;
using ReadMangaApp.View;
using System.Configuration;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace ReadMangaApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);// Создаем экземпляр DBConnection 
            DataContext = new MainWindowVM(this, dbConnection);
            LoadInitialPage();
        }

        private void LoadInitialPage()
        {
            MainContent.Navigate(new MainMangaPage(this)); // Передаем this — текущий MainWindow
        }
    }
}
