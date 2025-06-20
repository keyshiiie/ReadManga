using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.DataAccess
{
    public class ApiClientsBundle
    {
        public MangaApiClient MangaApiClient { get; }
        public PublisherApiClient PublisherApiClient { get; }
        public GenreApiClient GenreApiClient { get; }
        public TegApiClient TegApiClient { get; }
        public MangaScoreApiClient MangaScoreApiClient { get; }
        public AuthApiClient AuthApiClient { get; }
        public MangaCollectionApiClient MangaCollectionApiClient { get; }
        public ChapterApiClient ChapterApiClient { get; }
        public PageApiClient PageApiClient { get; }

        public ApiClientsBundle(HttpClient httpClient)
        {
            MangaApiClient = new MangaApiClient(httpClient);
            PublisherApiClient = new PublisherApiClient(httpClient);
            GenreApiClient = new GenreApiClient(httpClient);
            TegApiClient = new TegApiClient(httpClient);
            MangaScoreApiClient = new MangaScoreApiClient(httpClient);
            AuthApiClient = new AuthApiClient(httpClient);
            MangaCollectionApiClient = new MangaCollectionApiClient(httpClient);
            ChapterApiClient = new ChapterApiClient(httpClient);
            PageApiClient = new PageApiClient(httpClient);
        }
    }

}
