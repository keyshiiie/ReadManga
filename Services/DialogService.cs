using ReadMangaApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Services
{
    public interface IDialogService
    {
        bool? ShowLoginDialog();
    }

    public class DialogService : IDialogService
    {
        public bool? ShowLoginDialog()
        {
            var loginWindow = new LoginWindow();
            return loginWindow.ShowDialog();
        }
    }
}
