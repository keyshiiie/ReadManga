using ReadMangaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Dtos
{
    public class ChapterReadPageParams
    {
        public Chapter chapter { get; }
        public List<Chapter> Chapters { get; }

        public ChapterReadPageParams(List<Chapter> chapters, Chapter selectedChapter)
        {
            chapter = selectedChapter;
            Chapters = chapters;
        }
    }
}
