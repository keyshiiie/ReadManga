using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Services
{
    public static class CollectionChangedNotifier
    {
        public static event Action? CollectionsChanged;

        public static void NotifyCollectionsChanged()
        {
            CollectionsChanged?.Invoke();
        }
    }

}
