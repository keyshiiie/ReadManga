namespace ReadMangaApp.Models
{
    public class StatusTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public StatusTranslation(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
