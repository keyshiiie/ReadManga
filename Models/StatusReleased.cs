namespace ReadMangaApp.Models
{
    public class StatusReleased
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public StatusReleased(int id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}
