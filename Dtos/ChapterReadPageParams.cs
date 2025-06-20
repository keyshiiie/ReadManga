using ReadMangaApp.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ReadMangaApp.Dtos
{
    public class ChapterReadPageParams
    {
        public Chapter Chapter { get; }
        public List<Chapter> Chapters { get; }
        public List<MangaPage> Pages { get; }  // новое свойство

        public ChapterReadPageParams(List<Chapter> chapters, Chapter selectedChapter, List<MangaPage> pages)
        {
            Chapter = selectedChapter;
            Chapters = chapters;
            Pages = pages;
        }
    }
}
