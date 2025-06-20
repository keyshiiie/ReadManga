using ReadMangaApp.DataAccess;
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
        public IEnumerable<Chapter> Chapters { get; }
        public PageApiClient PageApiClient { get; }

        public ChaptersPageParams(IEnumerable<Chapter> chapters, PageApiClient pageApiClient)
        {
            Chapters = chapters;
            PageApiClient = pageApiClient;
        }
    }
}
