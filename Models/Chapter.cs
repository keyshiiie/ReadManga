using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public Manga? Manga { get; set; }
        public string ChapterTitle { get; set; }
        public DateTime DatePublished { get; set; }
        public int ChapterNumber { get; set; }

        public Chapter(int id, Manga? manga, string chapterTitle, DateTime datePublished, int chapterNumber)
        {
            Id = id;
            Manga = manga;
            ChapterTitle = chapterTitle;
            DatePublished = datePublished;
            ChapterNumber = chapterNumber;
        }
    }
}
