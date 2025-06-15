namespace ReadMangaApp.Models
{
    public class MangaScores
    {
        public int IdManga { get; set; }
        public decimal AverageScore { get; set; }

        public MangaScores(int idManga, decimal averageScore)
        {
            IdManga = idManga;
            AverageScore = averageScore;
        }
    }
}
