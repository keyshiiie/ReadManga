using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Repository;
using ReadMangaApp.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    public class MainMangaPageVM
    {
        private readonly MainWindow _mainWindow;
        private readonly DBConnection _dbConnection;
        private List<Manga> _allMangas;
        private ObservableCollection<Publisher> _publisher;
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
        private ObservableCollection<Teg> _tegs;
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
        private ObservableCollection<Genre> _genres;
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
            private set { _mangas = value; }
        }
        public ICommand ReadMangaCommand { get; }
        public ICommand SortMangaCommand { get; }
        public ICommand CancelFiltersCommnad { get; }

        public MainMangaPageVM(MainMangaPage mainMangaPage, MainWindow mainWindow, DBConnection dbConnection)
        {
            _mainWindow = mainWindow;
            _dbConnection = dbConnection;
            _allMangas = new List<Manga>();

            LoadAllMangaData();

            _mangas = new ObservableCollection<Manga>(_allMangas);
            ReadMangaCommand = new RelayCommand<Manga>(manga => ReadManga(manga));
            SortMangaCommand = new RelayCommand<object>(_ => SortManga());
            CancelFiltersCommnad = new RelayCommand<object>(_ => CancelFilters());
            _genres = new ObservableCollection<Genre>(GenreRepository.GetAllGenre(_dbConnection) ?? new List<Genre>());
            _tegs = new ObservableCollection<Teg>(TegRepository.GetAllTegs(_dbConnection) ?? new List<Teg>());
            _publisher = new ObservableCollection<Publisher>(PublisherRepository.GetAllPublisher(_dbConnection) ?? new List<Publisher>());
        }
        private void LoadAllMangaData()
        {
            // 1. Получаем все манги
            var mangas = MangaRepository.GetAllManga(_dbConnection);
            // 2. Получаем все жанры по манге
            var genresByManga = GenreRepository.GetAllGenresByAllManga(_dbConnection);
            // 3. Получаем все теги по манге
            var tegsByManga = TegRepository.GetAllTegsByAllManga(_dbConnection);
            // 4. Получаем все средние оценки по манге
            var scoresByManga = MangaScoreRepository.GetAllAverageScores(_dbConnection);
            // 5. Присваиваем данные каждой манге
            foreach (var manga in mangas)
            {
                manga.Genres = genresByManga.TryGetValue(manga.Id, out var genres) ? genres : new List<Genre>();
                manga.Tegs = tegsByManga.TryGetValue(manga.Id, out var tegs) ? tegs : new List<Teg>();
                manga.MangaScores = new MangaScores(manga.Id, scoresByManga.TryGetValue(manga.Id, out var score) ? score : 0);
            }
            _allMangas = mangas;
        }
        private void SortManga()
        {
            var selectedGenres = Genres.Where(g => g.IsSelected).Select(g => g.Id).ToList();
            var selectedTegs = Tegs.Where(t => t.IsSelected).Select(t => t.Id).ToList();
            var selectedPublishers = Publishers.Where(p => p.IsSelected).Select(p => p.Id).ToList();
            FilterMangas(selectedGenres, selectedTegs, selectedPublishers);
        }
        private void ReadManga(Manga selectedManga)
        {
            // Передаем всю информацию о манге, включая жанры, теги и средние оценки
            var mangaDetailPage = new MangaDetailPage(selectedManga, selectedManga.Genres, selectedManga.Tegs, selectedManga.MangaScores, _mainWindow);
            _mainWindow.MainContent.Navigate(mangaDetailPage);
        }
        public void FilterMangas(List<int> selectedGenres, List<int> selectedTegs, List<int> selectedPublishers)
        {
            var filtered = _allMangas.Where(manga =>
            (selectedGenres.Count == 0 || manga.Genres.Any(genre => selectedGenres.Contains(genre.Id))) &&
            (selectedTegs.Count == 0 || manga.Tegs.Any(teg => selectedTegs.Contains(teg.Id))) &&
            (selectedPublishers.Count == 0 || manga.Tegs.Any(publisher => selectedPublishers.Contains(publisher.Id)))
            ).ToList();
            Mangas.Clear();
            foreach (var manga in filtered)
            {
                Mangas.Add(manga);
            }
            if (Mangas.Count == 0)
            {
                MessageBox.Show("Манга по данному запросу не найдена.", "Результат сортировки", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void CancelFilters()
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
