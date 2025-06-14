using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Repository;
using ReadMangaApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class ChapterReadPageVM : ViewModelBase
    {
        private ObservableCollection<MangaPage> _pages;
        private ObservableCollection<Chapter> _chapters;
        private readonly DBConnection _dbConnection;
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
                    // Сохраняем текущую главу для проверки
                    var previousChapter = _selectedChapter;
                    _selectedChapter = value;
                    _currentPageIndex = 0; // Сброс индекса страницы на 0
                    if (!LoadPages())
                    {
                        // Если страницы не загружены, возвращаемся к предыдущей главе
                        SelectedChapter = previousChapter;
                        AppServices.DialogService.ShowMessage("В выбранной главе нет страниц.", "Ошибка");
                    }
                    else
                    {
                        OnPropertyChanged(nameof(CurrentPage)); // Уведомляем об изменении текущей страницы
                    }

                    OnPropertyChanged(nameof(SelectedChapter));
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
                OnPropertyChanged(nameof(CurrentPage)); // Обновляем текущую страницу при изменении коллекции
            }
        }
        public MangaPage? CurrentPage => (Pages != null && _currentPageIndex >= 0 && _currentPageIndex < Pages.Count) ? Pages[_currentPageIndex] : null;
        public ICommand GoBackCommand { get; }
        public ICommand GoForwardCommand { get; }

        public ChapterReadPageVM(Chapter selectedChapter, List<Chapter> chapters, DBConnection dbConnection)
        {
            _selectedChapter = selectedChapter;
            _chapters = new ObservableCollection<Chapter>(chapters); // Инициализируем коллекцию глав
            _dbConnection = dbConnection;
            _pages = Pages = new ObservableCollection<MangaPage>();
            _currentPageIndex = 0; // Сбрасываем индекс текущей страницы
            GoBackCommand = new RelayCommand(GoBack, CanGoBack);
            GoForwardCommand = new RelayCommand(GoForward, CanGoForward);
            LoadPages(); // Загружаем страницы для начальной главы
        }
        private bool LoadPages()
        {
            try
            {
                var allPages = GetPagesFromDatabase(); // Получаем страницы из базы данных
                if (allPages == null || !allPages.Any())
                {
                    Pages = new ObservableCollection<MangaPage>(); // Устанавливаем пустую коллекцию
                    return false; // Возвращаем false, если страниц нет
                }

                Pages = new ObservableCollection<MangaPage>(allPages); // Обновляем коллекцию страниц
                if (Pages.Count > 0)
                {
                    _currentPageIndex = 0; // Устанавливаем текущую страницу на первую
                }

                OnPropertyChanged(nameof(CurrentPage)); // Обновляем текущую страницу после загрузки
                return true; // Возвращаем true, если страницы успешно загружены
            }
            catch (Exception ex)
            {
                // Обработка исключения (например, вывести сообщение об ошибке)
                AppServices.DialogService.ShowMessage($"Ошибка при загрузке страниц: {ex.Message}", "Ошибка");
                return false; // Возвращаем false при ошибке
            }
        }
        private List<MangaPage> GetPagesFromDatabase()
        {
            return PagesRepository.GetAllPages(_dbConnection, SelectedChapter.Id); // Получаем страницы по ID главы
        }
        private void GoBack()
        {
            if (CanGoBack())
            {
                _currentPageIndex--;
                OnPropertyChanged(nameof(CurrentPage)); // Уведомляем об изменении текущей страницы
            }
        }
        private void GoForward()
        {
            if (CanGoForward())
            {
                _currentPageIndex++;
                OnPropertyChanged(nameof(CurrentPage)); // Уведомляем об изменении текущей страницы
            }
        }
        private bool CanGoBack() => _currentPageIndex > 0; // Проверка возможности вернуться назад
        private bool CanGoForward() => _currentPageIndex < (Pages?.Count - 1); // Проверка возможности перейти вперед
        
    }
}
