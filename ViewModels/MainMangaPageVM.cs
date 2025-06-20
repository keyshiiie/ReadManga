using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    public class MainMangaPageVM : ViewModelBase
    {
        private readonly MangaCollectionApiClient _mangaCollectionApiClient;
        private readonly INavigationService _navigationService;
        private readonly MangaApiClient _mangaApiClient; // Используем MangaApiClient вместо DBConnection
        private readonly PublisherApiClient _publisherApiClient;
        private readonly GenreApiClient _genreApiClient;
        private readonly TegApiClient _tegApiClient;
        private readonly MangaScoreApiClient _mangaScoreApiClient;

        private Dictionary<int, string> _collectionsByManga = new Dictionary<int, string>();
        private List<Manga> _allMangas;

        private ObservableCollection<Publisher> _publishers = new ObservableCollection<Publisher>();
        public ObservableCollection<Publisher> Publishers
        {
            get => _publishers;
            private set
            {
                if (_publishers != value)
                {
                    _publishers = value;
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

        private ObservableCollection<Manga> _mangas = new ObservableCollection<Manga>();
        public ObservableCollection<Manga> Mangas
        {
            get => _mangas;
            private set
            {
                if (_mangas != value)
                {
                    _mangas = value;
                    OnPropertyChanged(nameof(Mangas));
                }
            }
        }


        public ICommand ReadMangaCommand { get; }
        public ICommand SortMangaCommand { get; }
        public ICommand CancelFiltersCommand { get; }

        public MainMangaPageVM(INavigationService navigationService, MangaApiClient mangaApiClient, PublisherApiClient publisherApiClient, GenreApiClient genreApiClient, TegApiClient tegApiClient, MangaScoreApiClient mangaScoreApiClient, MangaCollectionApiClient mangaCollectionApiClient)
        {
            _mangaCollectionApiClient = mangaCollectionApiClient;
            _navigationService = navigationService;
            _mangaApiClient = mangaApiClient;
            _publisherApiClient = publisherApiClient;
            _genreApiClient = genreApiClient;
            _tegApiClient = tegApiClient;
            _mangaScoreApiClient = mangaScoreApiClient;

            _allMangas = new List<Manga>();
            _ = LoadAllMangaDataAsync(); // Загрузка данных с сервера
            _ = LoadCollectionsForMangasAsync();

            LoadPublishers();
            LoadGenres();
            LoadTegs();

            ReadMangaCommand = new RelayCommand<Manga>(manga => ReadManga(manga));
            SortMangaCommand = new RelayCommand<object>(_ => SortManga());
            CancelFiltersCommand = new RelayCommand<object>(_ => CancelFilters());

            UserSession.Instance.UserChanged += async (s, user) =>
            {
                await LoadCollectionsForMangasAsync();
                UpdateMangasCollections();
                RefreshMangasObservableCollection();
            };

            CollectionChangedNotifier.CollectionsChanged += async () =>
            {
                await LoadCollectionsForMangasAsync();
                UpdateMangasCollections();
                RefreshMangasObservableCollection();
            };
        }
        // Загружает все манги и связанные с ними данные из базы данных
        // _allMangas содержит полные данные по каждой манге
        private async Task LoadAllMangaDataAsync()
        {
            try
            {
                // 1. Получаем все манги
                var mangas = await _mangaApiClient.GetAllMangaAsync();

                // 2. Получаем все жанры по манге
                var genresByManga = await _genreApiClient.GetAllGenresByAllMangaAsync();

                // 3. Получаем все теги по манге
                var tegsByManga = await _tegApiClient.GetAllTegsByAllMangaAsync();

                // 4. Получаем все средние оценки по манге
                var scoresByManga = await _mangaScoreApiClient.GetAllAverageScoresAsync();

                // 5. Получаем все издательства по манге
                var publishersByManga = await _publisherApiClient.GetAllPublishersByAllMangaAsync();

                // 6. Присваиваем данные каждой манге
                foreach (var manga in mangas)
                {
                    manga.Genres.AddRange(genresByManga.TryGetValue(manga.Id, out var genres) ? genres : new List<Genre>());
                    manga.Tegs.AddRange(tegsByManga.TryGetValue(manga.Id, out var tegs) ? tegs : new List<Teg>());
                    manga.MangaScores = new MangaScores(manga.Id, scoresByManga.TryGetValue(manga.Id, out var score) ? score : 0);
                    manga.Publishers.AddRange(publishersByManga.TryGetValue(manga.Id, out var publishers) ? publishers : new List<Publisher>());

                }
                _allMangas = mangas;
                Mangas.Clear();
                foreach (var manga in mangas)
                {
                    Mangas.Add(manga);
                }

                OnPropertyChanged(nameof(Mangas));
                SortManga();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // загрузка издательств для фильтрации
        private async void LoadPublishers()
        {
            try
            {
                var publishers = await _publisherApiClient.GetAllPublisherAsync();
                Publishers.Clear();
                foreach (var publisher in publishers)
                {
                    Publishers.Add(publisher);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw; // пробрасываем исходное исключение дальше
            }
        }

        private async void LoadGenres()
        {
            try
            {
                var genres = await _genreApiClient.GetAllGenresAsync();
                Genres.Clear();
                foreach(var genre in genres)
                {
                    Genres.Add(genre);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw; // пробрасываем исходное исключение дальше
            }
        }

        private async void LoadTegs()
        {
            try
            {
                var tegs = await _tegApiClient.GetAllTegsAsync();
                Tegs.Clear();
                foreach (var teg in tegs)
                {
                    Tegs.Add(teg);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                throw; // пробрасываем исходное исключение дальше
            }
        }

        // Обрабатывает нажатие кнопки "Соритировать" т.е собирает выбранные фильтры по id
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
                AppServices.DialogService.ShowMessage($"Ошибка при выборе фильтров: {ex.Message}");
            }
        }
        // Фильтрует _allMangas по выбранным фильтрам
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
                    AppServices.DialogService.ShowMessage("Манга по данному запросу не найдена.");
                }
            }
            catch (Exception ex)
            {
                AppServices.DialogService.ShowMessage($"Ошибка при фильтрации манги: {ex.Message}");
            }
        }
        // Сбрасывает все фильтры
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
                AppServices.DialogService.ShowMessage($"Ошибка при сбросе фильтров: {ex.Message}");
            }
        }

         // Открывает страницу с подробной информацией о выбранной манге
        private void ReadManga(Manga selectedManga)
        {
            if (selectedManga == null)
            {
                AppServices.DialogService.ShowMessage("Выберите мангу для чтения.");
                return;
            }
            _navigationService.NavigateTo("MangaDetailPage", selectedManga);
        }

        // Загружает коллекции пользователя
        // _collectionsByManga содержит коллекции пользователя
        private async Task LoadCollectionsForMangasAsync()
        {
            try
            {
                var user = UserSession.Instance.CurrentUser;
                if (user != null)
                {
                    var collections = await _mangaCollectionApiClient.GetCollectionsByMangaForUserAsync();
                    _collectionsByManga = collections;
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
        // Присваивает каждой манге название коллекции пользователя (если есть)
        // Каждая манга знает, в какой пользовательской коллекции она находится.
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
        // Обновляет отображаемую коллекцию манги (Mangas) из _allMangas
        // UI обновляется и показывает все манги
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
    }
}
