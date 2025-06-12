using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Models
{
    public class Chapter
    {
        private int _id;
        private Manga? _manga;
        private string _chapterTitle;
        private DateTime _datePublished;
        private int _chapterNumber;

        public Chapter(int id, Manga? manga,  string chapterTitle, DateTime datePublished, int chapterNumber)
        {
            _id = id;
            _manga = manga;
            _chapterTitle = chapterTitle;
            _datePublished = datePublished;
            _chapterNumber = chapterNumber;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public Manga? Manga
        {
            get => _manga;
            set => _manga = value;
        }

        public string ChapterTitle
        {
            get => _chapterTitle;
            set => _chapterTitle = value;
        }

        public DateTime DatePublished
        {
            get => _datePublished;
            set => _datePublished = value;
        }

        public int ChapterNumber
        {
            get => _chapterNumber;
            set => _chapterNumber = value;
        }
    }
}
