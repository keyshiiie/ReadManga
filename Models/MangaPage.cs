using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Models
{
    public class MangaPage
    {
        private int _id;
        private Chapter? _chapter;
        private int _pageNumber;
        private string _contentUrl;

        public MangaPage(int id, Chapter? chapter, int pageNumber, string contentUrl)
        {
            _id = id;
            _chapter = chapter;
            _pageNumber = pageNumber;
            _contentUrl = contentUrl;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public Chapter? Chapter
        {
            get => _chapter;
            set => _chapter = value;
        }

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value;
        }

        public string ContentUrl
        {
            get => _contentUrl;
            set => _contentUrl = value;
        }
    }
}
