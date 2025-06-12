using ReadMangaApp.Models;
using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Repository;
using ReadMangaApp.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    public class MangaDetailPageVM : INotifyPropertyChanged
    {
        private readonly MangaDetailPage _mangaDetailPage;
        private readonly MainWindow _mainWindow;
        private RateMangaPage? _rateWindow;
        private readonly DBConnection _dbConnection; // Добавляем поле для подключения к БД
        public Manga SelectedManga { get; }

        private ObservableCollection<Publisher> _publishers;
        public ObservableCollection<Publisher> Publishers
        {
            get => _publishers;
            private set
            {
                _publishers = value;
                OnPropertyChanged(nameof(SelectedManga));
            }
        }

        private ObservableCollection<Teg> _tegs;
        public ObservableCollection<Teg> Tegs 
        {
            get => _tegs;
            private set
            {
                _tegs = value;
                OnPropertyChanged(nameof(SelectedManga));
            }
        }

        private ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            private set
            {
                _genres = value;
                OnPropertyChanged(nameof(SelectedManga));
            }
        }

        private ObservableCollection<Chapter> _chapters;
        public ObservableCollection<Chapter> Chapters
        {
            get => _chapters;
            private set
            {
                _chapters = value;
                OnPropertyChanged(nameof(Chapters));
            }
        }

        private ObservableCollection<MangaCollection> _collections;
        public ObservableCollection<MangaCollection> Collections
        {
            get => _collections;
            set
            {
                _collections = value;
                OnPropertyChanged(nameof(Collections));
            }
        }

        private int? _selectedCollectionId;
        public int? SelectedCollectionId
        {
            get => _selectedCollectionId;
            set
            {
                _selectedCollectionId = value;
                OnPropertyChanged(nameof(SelectedCollectionId));
                // Обновляем SelectedCollection на основе выбранного Id
                SelectedCollection = Collections.FirstOrDefault(c => c.Id == _selectedCollectionId);
            }
        }

        private MangaCollection? _selectedCollection;
        public MangaCollection? SelectedCollection
        {
            get => _selectedCollection;
            set
            {
                _selectedCollection = value;
                OnPropertyChanged(nameof(SelectedCollection));
            }
        }
        public MangaScores? MangaScores { get; set; }  // Делаем nullable

        public decimal AverageScore => MangaScores?.AverageScore ?? 0.0m; // m указывает на decimal

        // команды для кнопок
        public ICommand OpenScorePageCommand { get; }
        public ICommand OpenMangaInfoPageCommand { get; }
        public ICommand OpenChaptersPageCommand { get; }
        public ICommand AddToCollectionCommand { get; }

        public MangaDetailPageVM(MangaDetailPage mangaDetailPage, Manga selectedManga, List<Genre> genres, List<Teg> tegs, MangaScores? mangaScores, MainWindow mainWindow, DBConnection dbConnection)
        {
            _mangaDetailPage = mangaDetailPage;
            _mainWindow = mainWindow;
            SelectedManga = selectedManga;
            MangaScores = mangaScores;
            _collections = new ObservableCollection<MangaCollection>();
            _chapters = new ObservableCollection<Chapter>();
            _genres = new ObservableCollection<Genre>(genres);
            _tegs = new ObservableCollection<Teg>(tegs);
            _publishers = new ObservableCollection<Publisher>();
            _dbConnection = dbConnection;

            OpenScorePageCommand = new RelayCommand<object>(_ => ScoreManga());
            OpenMangaInfoPageCommand = new RelayCommand<object>(_ => OpenMangaInfo());
            OpenChaptersPageCommand = new RelayCommand<object>(_ => OpenChaptersPage());
            AddToCollectionCommand = new RelayCommand<object>(_ => AddMangaToCollection());

            LoadChapters();
            LoadPublishers();

            // Подписка на событие изменения пользователя
            UserSession.Instance.UserChanged += OnUserChanged;
            if (UserSession.Instance.CurrentUser != null)
            {
                LoadCollections();
            }
        }

        private void AddMangaToCollection()
        {
            if (SelectedCollection != null && SelectedManga != null && UserSession.Instance.CurrentUser != null)
            {
                int mangaId = SelectedManga.Id; // Предполагается, что имеется Id у манги
                int collectionId = SelectedCollection.Id; // Берём ID для использования в репозитории
                int userId = UserSession.Instance.CurrentUser.Id; // Получаем ID текущего пользователя

                string? result = CollectionRepository.UpdateMangasCollection(_dbConnection, userId, mangaId, collectionId);

                // Обработка результата
                if (result == "added")
                {
                    string message = $"Манга '{SelectedManga.Name}' была добавлена в коллекцию '{SelectedCollection.Title}'.";
                    MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (result == "updated")
                {
                    string message = $"Манга '{SelectedManga.Name}' была обновлена в коллекции '{SelectedCollection.Title}'.";
                    MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Логика для обработки ошибок
                    MessageBox.Show("Произошла ошибка при добавлении манги в коллекцию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Логика для обработки случая, когда коллекция или манга не выбраны, или пользователь не авторизован
                MessageBox.Show("Пожалуйста, выберите коллекцию и мангу, а также убедитесь, что вы авторизованы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void OnUserChanged(object? sender, User? user)
        {
            if (user != null)
            {
                LoadCollections();
            }
            else
            {
                // Очистить коллекции при выходе пользователя
                Collections = new ObservableCollection<MangaCollection>();
            }
        }


        private void LoadCollections()
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                Collections = new ObservableCollection<MangaCollection>();
                return;
            }
            var user = UserSession.Instance.CurrentUser;
            var collectionsList = CollectionRepository.GetAllCollectionsByUser(_dbConnection, user.Id, user);
            Collections = new ObservableCollection<MangaCollection>(collectionsList);
        }



        private void OpenMangaInfo()
        {
            var mangaInfoPage = new MangaInfoPage(_mangaDetailPage, SelectedManga, Genres, Tegs);
            _mangaDetailPage.MangaDetailContent.Navigate(mangaInfoPage);
        }

        private void OpenChaptersPage()
        {
            var mangaChaptersPage = new ChaptersPage(_mainWindow, _mangaDetailPage, Chapters);
            _mangaDetailPage.MangaDetailContent.Navigate(mangaChaptersPage);
        }

        private void ScoreManga()
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (_rateWindow == null)
                {
                    _rateWindow = new RateMangaPage(SelectedManga);
                    _rateWindow.Closed += (s, e) => _rateWindow = null;
                    _rateWindow.ShowDialog();
                }
            }
        }

        private void LoadChapters()
        {
            var allChapters = GetChaptersFromDatabase(); // Получаем главы из базы данных
            Chapters = new ObservableCollection<Chapter>(allChapters); // Обновляем коллекцию глав
        }
        private void LoadPublishers()
        {
            var allPublisher = GetPublisherFromDatabase();
            Publishers = new ObservableCollection<Publisher>(allPublisher);
        }
        private List<Publisher> GetPublisherFromDatabase()
        {
            return PublisherRepository.GetAllPublisherByMangaId(_dbConnection, SelectedManga.Id);
        }
        private List<Chapter> GetChaptersFromDatabase()
        {
            return ChapterRepository.GetAllChapter(_dbConnection, SelectedManga.Id); // Получаем главы по ID манги
        }
        

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
