namespace ReadMangaApp.Models
{
    public class TypeManga
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TypeManga(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}