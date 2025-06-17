
namespace ReadMangaApp.Models
{
    public class Manga
    {
        public int Id { get; }
        public string Name { get; private set; }
        public int DatePublished { get; }
        public string CoverUrl { get; private set; }
        public StatusReleased StatusReleased { get; }
        public StatusTranslation StatusTranslation { get; }
        public TypeManga TypeManga { get; }
        public string? Author { get; private set; }
        public string? Description { get; private set; }
        public string? AlternativeTitle { get; private set; }
        public string? Collection { get; set; }

        public List<Teg> Tegs { get; } = new();
        public List<Genre> Genres { get; } = new();
        public List<Publisher> Publishers { get; } = new();

        public MangaScores? MangaScores { get; set; }

        public decimal AverageScore => MangaScores?.AverageScore ?? 0.0m;

        public Manga(
            int id,
            string name,
            int datePublished,
            string coverUrl,
            StatusReleased statusReleased,
            StatusTranslation statusTranslation,
            TypeManga type,
            string? author,
            string? description,
            string? alternativeTitle)
        {
            Id = id;
            Name = name;
            DatePublished = datePublished;
            CoverUrl = coverUrl;
            StatusReleased = statusReleased;
            StatusTranslation = statusTranslation;
            TypeManga = type;
            Author = author;
            Description = description;
            AlternativeTitle = alternativeTitle;
        }
    }
}
