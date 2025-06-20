using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class ChapterReadPageVM : ViewModelBase
    {
        private readonly PageApiClient _pageApiClient;
        private ObservableCollection<MangaPage> _pages = new ObservableCollection<MangaPage>();
        private ObservableCollection<Chapter> _chapters = new ObservableCollection<Chapter>();
        private int _currentPageIndex;
        private Chapter _selectedChapter;

        public ObservableCollection<Chapter> Chapters
        {
            get => _chapters;
            set
            {
                _chapters = value;
                OnPropertyChanged(nameof(Chapters));
            }
        }

        public Chapter SelectedChapter
        {
            get => _selectedChapter;
            set
            {
                if (_selectedChapter != value)
                {
                    var previousChapter = _selectedChapter;
                    _selectedChapter = value;
                    _currentPageIndex = 0;
                    OnPropertyChanged(nameof(SelectedChapter));
                    // Запускаем асинхронную загрузку страниц, не блокируя UI
                    _ = LoadPagesAsyncWithFallback(previousChapter);
                }
            }
        }

        public ObservableCollection<MangaPage> Pages
        {
            get => _pages;
            private set
            {
                _pages = value;
                OnPropertyChanged(nameof(Pages));
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public MangaPage? CurrentPage => (Pages != null && _currentPageIndex >= 0 && _currentPageIndex < Pages.Count) ? Pages[_currentPageIndex] : null;

        public ICommand GoBackCommand { get; }
        public ICommand GoForwardCommand { get; }

        public ChapterReadPageVM(Chapter selectedChapter, List<Chapter> chapters, List<MangaPage> pages, PageApiClient pageApiClient)
        {
            _pageApiClient = pageApiClient;
            _selectedChapter = selectedChapter;
            _chapters = new ObservableCollection<Chapter>(chapters);
            Pages = new ObservableCollection<MangaPage>(pages);
            _currentPageIndex = 0;

            GoBackCommand = new RelayCommand(GoBack, CanGoBack);
            GoForwardCommand = new RelayCommand(GoForward, CanGoForward);
        }

        private async Task<bool> LoadPagesAsync(Chapter chapter)
        {
            var pages = await _pageApiClient.GetAllChapterPagesAsync(chapter.Id);

            if (pages == null || !pages.Any())
            {
                return false;
            }

            Pages = new ObservableCollection<MangaPage>(pages);
            _currentPageIndex = 0;
            OnPropertyChanged(nameof(CurrentPage));
            return true;
        }

        private async Task LoadPagesAsyncWithFallback(Chapter previousChapter)
        {
            bool success = await LoadPagesAsync(_selectedChapter);
            if (!success)
            {
                // Если не удалось загрузить страницы, возвращаем предыдущую главу
                _selectedChapter = previousChapter;
                OnPropertyChanged(nameof(SelectedChapter));
                AppServices.DialogService.ShowMessage("В выбранной главе нет страниц.", "Ошибка");
                // Восстанавливаем страницы предыдущей главы
                await LoadPagesAsync(previousChapter);
            }
        }

        private void GoBack()
        {
            if (CanGoBack())
            {
                _currentPageIndex--;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private void GoForward()
        {
            if (CanGoForward())
            {
                _currentPageIndex++;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private bool CanGoBack() => _currentPageIndex > 0;
        private bool CanGoForward() => _currentPageIndex < (Pages?.Count - 1);
    }
}
