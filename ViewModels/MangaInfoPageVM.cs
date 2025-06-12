using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.ViewModels
{
    class MangaInfoPageVM
    {
        public Manga SelectedManga { get; }
        public IEnumerable<Genre> Genres { get; }
        public IEnumerable<Teg> Tegs { get; }

        private readonly MangaDetailPage _mangaDetailPage;

        public MangaInfoPageVM(MangaDetailPage mangaDetailPage, DBConnection dBConnection, Manga selectedManga, IEnumerable<Genre> genres, IEnumerable<Teg> tegs)
        {
            _mangaDetailPage = mangaDetailPage;
            SelectedManga = selectedManga;
            Genres = genres;
            Tegs = tegs;
        }
    }
}
