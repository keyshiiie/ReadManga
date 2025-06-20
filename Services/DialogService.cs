using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.View;
using System.Data;
using System.Data.Common;

namespace ReadMangaApp.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title = "Информация");
        bool? ShowRateDialog(Manga selectedManga, MangaScoreApiClient mangaScoreApiClient);
        bool? ShowLoginDialog(AuthApiClient authApiClient);
    }

    public class DialogService : IDialogService
    {
        public bool? ShowLoginDialog(AuthApiClient authApiClient)
        {
            var loginWindow = new LoginWindow(authApiClient);
            return loginWindow.ShowDialog();
        }

        public void ShowMessage(string message, string title = "Информация")
        {
            System.Windows.MessageBox.Show(message, title);
        }

        public bool? ShowRateDialog(Manga selectedManga, MangaScoreApiClient mangaScoreApiClient)
        {
            var rateWindow = new RateMangaWindow(selectedManga, mangaScoreApiClient);
            return rateWindow.ShowDialog();
        }
    }
}
