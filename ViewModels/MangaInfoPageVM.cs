using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;

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
