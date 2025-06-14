using ReadMangaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Dtos
{
    public class ChaptersPageParams
    {
        public List<Chapter> Chapters { get; }

        public ChaptersPageParams(List<Chapter> chapters)
        {
            Chapters = chapters;
        }
    }
}
