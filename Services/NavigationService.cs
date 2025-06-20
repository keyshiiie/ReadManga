using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ReadMangaApp.Services
{
    public interface INavigationService
    {
        void NavigateTo(string pageKey);
        void NavigateTo(string pageKey, object parameter);
        void GoBack();
        void GoForward();
    }

    public class FrameNavigationService : INavigationService
    {
        private readonly Frame _frame;

        private readonly Dictionary<string, Func<Page>> _pageFactories = new();
        private readonly Dictionary<string, Func<object, Page>> _pageFactoriesWithParam = new();

        // Кэш для страниц без параметров
        private readonly Dictionary<string, Page> _pageCache = new();

        // Кэш для страниц с параметром — сложнее, можно кэшировать по ключу + параметру, если параметры повторяются
        // Для простоты можно не кэшировать такие страницы или кэшировать только по ключу, если параметр не меняется
        // В вашем случае MangaDetailPage зависит от параметра, поэтому лучше не кэшировать или реализовать сложнее

        public FrameNavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void Configure(string key, Func<Page> factory)
        {
            _pageFactories[key] = factory;
        }

        public void Configure(string key, Func<object, Page> factory)
        {
            _pageFactoriesWithParam[key] = factory;
        }

        public void NavigateTo(string pageKey)
        {
            if (!_pageFactories.ContainsKey(pageKey))
                throw new ArgumentException($"No such page: {pageKey}");

            if (!_pageCache.TryGetValue(pageKey, out var page))
            {
                page = _pageFactories[pageKey]();
                _pageCache[pageKey] = page;
            }

            _frame.Navigate(page);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            if (!_pageFactoriesWithParam.ContainsKey(pageKey))
                throw new ArgumentException($"No such page: {pageKey}");

            // Для страниц с параметром не кэшируем, чтобы не путать данные
            var page = _pageFactoriesWithParam[pageKey](parameter);
            _frame.Navigate(page);
        }

        public void GoBack()
        {
            if (_frame.CanGoBack) _frame.GoBack();
        }

        public void GoForward()
        {
            if (_frame.CanGoForward) _frame.GoForward();
        }
    }

}
