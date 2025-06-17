using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Repository;

namespace ReadMangaApp.Services
{
    public class MangaService
    {
        private readonly DBConnection _dbConnection;

        public MangaService(DBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Manga> LoadAllMangaData()
        {
            var mangas = MangaRepository.GetAllManga(_dbConnection);
            var genresByManga = GenreRepository.GetAllGenresByAllManga(_dbConnection);
            var tegsByManga = TegRepository.GetAllTegsByAllManga(_dbConnection);
            var scoresByManga = MangaScoreRepository.GetAllAverageScores(_dbConnection);
            var publishersByManga = PublisherRepository.GetAllPublishersByAllManga(_dbConnection);

            foreach (var manga in mangas)
            {
                manga.Genres.AddRange(genresByManga.TryGetValue(manga.Id, out var genres) ? genres : new List<Genre>());
                manga.Tegs.AddRange(tegsByManga.TryGetValue(manga.Id, out var tegs) ? tegs : new List<Teg>());
                manga.MangaScores = new MangaScores(manga.Id, scoresByManga.TryGetValue(manga.Id, out var score) ? score : 0);
                manga.Publishers.AddRange(publishersByManga.TryGetValue(manga.Id, out var publishers) ? publishers : new List<Publisher>());
            }

            return mangas;
        }
    }

}
