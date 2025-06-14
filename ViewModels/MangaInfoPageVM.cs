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


        public MangaInfoPageVM(DBConnection dBConnection, Manga selectedManga, IEnumerable<Genre> genres, IEnumerable<Teg> tegs)
        {
            SelectedManga = selectedManga;
            Genres = genres;
            Tegs = tegs;
        }
    }
}
