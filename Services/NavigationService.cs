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
    }

    public class FrameNavigationService : INavigationService
    {
        private readonly Frame _frame;

        private readonly Dictionary<string, Func<Page>> _pageFactories = new();
        private readonly Dictionary<string, Func<object, Page>> _pageFactoriesWithParam = new();

        public FrameNavigationService(Frame frame)
        {
            _frame = frame;
        }

        // Регистрация страницы без параметров
        public void Configure(string key, Func<Page> factory)
        {
            _pageFactories[key] = factory;
        }

        // Регистрация страницы с параметром
        public void Configure(string key, Func<object, Page> factory)
        {
            _pageFactoriesWithParam[key] = factory;
        }

        public void NavigateTo(string pageKey)
        {
            if (!_pageFactories.ContainsKey(pageKey))
                throw new ArgumentException($"No such page: {pageKey}");

            var page = _pageFactories[pageKey]();
            _frame.Navigate(page);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            if (!_pageFactoriesWithParam.ContainsKey(pageKey))
                throw new ArgumentException($"No such page: {pageKey}");

            var page = _pageFactoriesWithParam[pageKey](parameter);
            _frame.Navigate(page);
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
                _frame.GoBack();
        }
    }
}
