using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.View;
using System.Data;
using System.Data.Common;

namespace ReadMangaApp.Services
{
    public interface IDialogService
    {
        bool? ShowLoginDialog(DBConnection dbConnection);
        bool? ShowRateDialog(Manga selectedManga, DBConnection dbConnection);
        void ShowMessage(string message, string title = "Информация");
    }

    public class DialogService : IDialogService
    {
        public bool? ShowLoginDialog(DBConnection dbConnection)
        {
            var loginWindow = new LoginWindow(dbConnection);
            return loginWindow.ShowDialog();
        }

        public bool? ShowRateDialog(Manga selectedManga, DBConnection dbConnection)
        {
            var rateWindow = new RateMangaWindow(selectedManga, dbConnection);
            return rateWindow.ShowDialog();
        }

        public void ShowMessage(string message, string title = "Информация")
        {
            System.Windows.MessageBox.Show(message, title);
        }
    }
}
