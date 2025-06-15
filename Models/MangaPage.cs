namespace ReadMangaApp.Models
{
    public class MangaPage
    {
        public int Id { get; set; }
        public Chapter? Chapter { get; set; }
        public int PageNumber { get; set; }
        public string ContentUrl { get; set; }

        public MangaPage(int id, Chapter? chapter, int pageNumber, string contentUrl)
        {
            Id = id;
            Chapter = chapter;
            PageNumber = pageNumber;
            ContentUrl = contentUrl;
        }
    }
}
