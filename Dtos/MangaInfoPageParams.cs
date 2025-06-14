using ReadMangaApp.Models;

namespace ReadMangaApp.Dtos
{
    public class MangaInfoPageParams
    {
        public Manga Manga { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Teg> Tegs { get; set; }

        public MangaInfoPageParams(Manga manga, List<Genre> genres, List<Teg> tegs)
        {
            Manga = manga;
            Genres = genres;
            Tegs = tegs;
        }
    }
}
