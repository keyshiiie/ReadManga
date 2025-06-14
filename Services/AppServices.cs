using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Services
{
    public static class AppServices
    {
        public static IDialogService DialogService { get; } = new DialogService();
    }
}
