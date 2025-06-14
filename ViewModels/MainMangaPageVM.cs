using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Repository;
using ReadMangaApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    public class MainMangaPageVM : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private readonly DBConnection _dbConnection;
        private List<Manga> _allMangas;

        private Dictionary<int, string> _collectionsByManga = new Dictionary<int, string>();

        private ObservableCollection<Publisher> _publisher = new ObservableCollection<Publisher>();
        public ObservableCollection<Publisher> Publishers
        {
            get => _publisher;
            private set 
            {
                if (_publisher != value)
                {
                    _publisher = value;
                    OnPropertyChanged(nameof(Publishers));
                }
            }
        }

        private ObservableCollection<Teg> _tegs = new ObservableCollection<Teg>();
        public ObservableCollection<Teg> Tegs
        {
            get => _tegs;
            private set 
            {
                if (_tegs != value)
                {
                    _tegs = value;
                    OnPropertyChanged(nameof(Tegs));
                }
            }
        }

        private ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();
        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            private set
            {
                if (_genres != value)
                {
                    _genres = value;
                    OnPropertyChanged(nameof(Genres));
                }
            }
        }

        private ObservableCollection<Manga> _mangas;
        public ObservableCollection<Manga> Mangas
        {
            get => _mangas;
            private set 
            { 
                if(_mangas != value)
                {
                    _mangas = value;
                    OnPropertyChanged(nameof(Mangas));
                }
            }
        }

        public ICommand ReadMangaCommand { get; }
        public ICommand SortMangaCommand { get; }
        public ICommand CancelFiltersCommand { get; }

        public MainMangaPageVM(INavigationService navigationService, IDialogService dialogService, DBConnection dbConnection)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            _dbConnection = dbConnection;
            _allMangas = new List<Manga>();

            LoadAllMangaData();
            LoadCollectionsForMangas();
            UpdateMangasCollections();

            _mangas = new ObservableCollection<Manga>(_allMangas);

            ReadMangaCommand = new RelayCommand<Manga>(manga => ReadManga(manga));
            SortMangaCommand = new RelayCommand<object>(_ => SortManga());
            CancelFiltersCommand = new RelayCommand<object>(_ => CancelFilters());

            LoadGenres();
            LoadTegs();
            LoadPublishers();

            // Подписка на смену пользователя, чтобы обновлять коллекции динамически
            UserSession.Instance.UserChanged += (s, user) =>
            {
                LoadCollectionsForMangas();
                UpdateMangasCollections();
                RefreshMangasObservableCollection();
            };
        }
        // загрузка жанров для сортировки
        private void LoadGenres()
        {
            try
            {
                var genresFromDb = GenreRepository.GetAllGenre(_dbConnection) ?? new List<Genre>();
                _genres.Clear();
                foreach (var genre in genresFromDb)
                {
                    _genres.Add(genre);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Ошибка при загрузке жанров: {ex.Message}");
            }
        }
        // загрузка тегов для сортировки
        private void LoadTegs()
        {
            try
            {
                var tegsFromDb = TegRepository.GetAllTegs(_dbConnection) ?? new List<Teg>();
                _tegs.Clear();
                foreach (var teg in tegsFromDb)
                {
                    _tegs.Add(teg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке тегов: {ex.Message}");
            }
            
        }

        // загрузка издательств для сортировки
        private void LoadPublishers()
        {
            try
            {
                var publishersFromDb = PublisherRepository.GetAllPublisher(_dbConnection) ?? new List<Publisher>();
                _publisher.Clear();
                foreach (var publisher in publishersFromDb)
                {
                    _publisher.Add(publisher);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Ошибка при загрузке издательств: {ex.Message}");
            }
        }
        // обновление каталога манги
        private void RefreshMangasObservableCollection()
        {
            try
            {
                Mangas.Clear();
                foreach (var manga in _allMangas)
                {
                    Mangas.Add(manga);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении коллекции манги: {ex.Message}");
            }
        }
        // загрузка коллекций манги
        private void LoadCollectionsForMangas()
        {
            try
            {
                var user = UserSession.Instance.CurrentUser;
                if (user != null)
                {
                    _collectionsByManga = MangaCollectionRepository.GetAllCollectionByManga(_dbConnection, user.Id);
                }
                else
                {
                    _collectionsByManga = new Dictionary<int, string>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке коллекций манги: {ex.Message}");
            }
        }
        // обновление информации о коллекциях
        private void UpdateMangasCollections()
        {
            try
            {
                foreach (var manga in _allMangas)
                {
                    if (_collectionsByManga.TryGetValue(manga.Id, out var collectionTitle))
                        manga.Collection = collectionTitle;
                    else
                        manga.Collection = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении коллекций манги: {ex.Message}");
            }
        }
        // загрузка данных на страницу
        private void LoadAllMangaData()
        {
            try
            {
                // 1. Получаем все манги
                var mangas = MangaRepository.GetAllManga(_dbConnection);
                // 2. Получаем все жанры по манге
                var genresByManga = GenreRepository.GetAllGenresByAllManga(_dbConnection);
                // 3. Получаем все теги по манге
                var tegsByManga = TegRepository.GetAllTegsByAllManga(_dbConnection);
                // 4. Получаем все средние оценки по манге
                var scoresByManga = MangaScoreRepository.GetAllAverageScores(_dbConnection);
                // 5. Получаем все издательства по манге
                var publishersByManga = PublisherRepository.GetAllPublishersByAllManga(_dbConnection);
                // 6. Присваиваем данные каждой манге
                foreach (var manga in mangas)
                {
                    manga.Genres = genresByManga.TryGetValue(manga.Id, out var genres) ? genres : new List<Genre>();
                    manga.Tegs = tegsByManga.TryGetValue(manga.Id, out var tegs) ? tegs : new List<Teg>();
                    manga.MangaScores = new MangaScores(manga.Id, scoresByManga.TryGetValue(manga.Id, out var score) ? score : 0);
                    manga.Publishers = publishersByManga.TryGetValue(manga.Id, out var publishers) ? publishers : new List<Publisher>();
                }
                _allMangas = mangas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
        // обработка выбранных пользователем фильтров
        private void SortManga()
        {
            try
            {
                var selectedGenres = Genres.Where(g => g.IsSelected).Select(g => g.Id).ToList();
                var selectedTegs = Tegs.Where(t => t.IsSelected).Select(t => t.Id).ToList();
                var selectedPublishers = Publishers.Where(p => p.IsSelected).Select(p => p.Id).ToList();
                FilterMangas(selectedGenres, selectedTegs, selectedPublishers);
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Ошибка при выборе фильтров: {ex.Message}");
            }
        }
        // фильтрация манги
        public void FilterMangas(List<int> selectedGenres, List<int> selectedTegs, List<int> selectedPublishers)
        {
            try
            {
                var filtered = _allMangas.Where(manga =>
                    (selectedGenres.Count == 0 || manga.Genres.Any(genre => selectedGenres.Contains(genre.Id))) &&
                    (selectedTegs.Count == 0 || manga.Tegs.Any(teg => selectedTegs.Contains(teg.Id))) &&
                    (selectedPublishers.Count == 0 || manga.Publishers.Any(publisher => selectedPublishers.Contains(publisher.Id)))
                ).ToList();
                Mangas.Clear();
                foreach (var manga in filtered)
                {
                    Mangas.Add(manga);
                }
                if (Mangas.Count == 0)
                {
                    _dialogService.ShowMessage("Манга по данному запросу не найдена.");
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Ошибка при фильтрации манги: {ex.Message}");
            }
        }
        // удаление выбранных фильтров
        public void CancelFilters()
        {
            try
            {
                foreach (var genre in Genres)
                {
                    genre.IsSelected = false;
                }
                foreach (var teg in Tegs)
                {
                    teg.IsSelected = false;
                }
                foreach (var publisher in Publishers)
                {
                    publisher.IsSelected = false;
                }
                FilterMangas(new List<int>(), new List<int>(), new List<int>());
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Ошибка при сбросе фильтров: {ex.Message}");
            }
        }
        // открытие страницы с детальной информацией
        private void ReadManga(Manga selectedManga)
        {
            if (selectedManga == null)
            {
                _dialogService.ShowMessage("Выберите мангу для чтения.");
                return;
            }
            _navigationService.NavigateTo("MangaDetailPage", selectedManga);
        }

    }
}
