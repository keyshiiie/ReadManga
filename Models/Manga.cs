
namespace ReadMangaApp.Models
{
    public class Manga
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CoverUrl { get; set; }
        public required StatusReleased StatusReleased { get; set; }
        public required StatusTranslation StatusTranslation { get; set; }
        public required TypeManga TypeManga { get; set; }
        public int DatePublished { get; set; } 
        public string? Author { get; set; } 
        public string? Description { get; set; } 
        public string? AlternativeTitle { get; set; } 
        public string? Collection { get; set; } 

        public List<Teg> Tegs { get; } = new();
        public List<Genre> Genres { get; } = new();
        public List<Publisher> Publishers { get; } = new();

        public MangaScores? MangaScores { get; set; }

        public decimal AverageScore => MangaScores?.AverageScore ?? 0.0m;

        // Конструктор по умолчанию
        public Manga() { }

        
    }
}
