using ReadMangaApp.View;

namespace ReadMangaApp.Services
{
    public interface IDialogService
    {
        bool? ShowLoginDialog();
        void ShowMessage(string message, string title = "Информация");
    }

    public class DialogService : IDialogService
    {
        public bool? ShowLoginDialog()
        {
            var loginWindow = new LoginWindow();
            return loginWindow.ShowDialog();
        }

        public void ShowMessage(string message, string title = "Информация")
        {
            System.Windows.MessageBox.Show(message, title);
        }
    }
}
