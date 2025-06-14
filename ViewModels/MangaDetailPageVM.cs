using ReadMangaApp.Models;
using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Repository;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReadMangaApp.Services;
using ReadMangaApp.Dtos;

namespace ReadMangaApp.ViewModels
{
    public class MangaDetailPageVM : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly DBConnection _dbConnection;
        public Manga SelectedManga { get; }

        private ObservableCollection<Publisher> _publishers = new ObservableCollection<Publisher>();
        public ObservableCollection<Publisher> Publishers
        {
            get => _publishers;
            private set
            {
                _publishers = value;
                OnPropertyChanged(nameof(SelectedManga));
            }
        }

        private ObservableCollection<Teg> _tegs = new ObservableCollection<Teg>();
        public ObservableCollection<Teg> Tegs 
        {
            get => _tegs;
            private set
            {
                _tegs = value;
                OnPropertyChanged(nameof(Tegs));
            }
        }

        private ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();
        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            private set
            {
                _genres = value;
                OnPropertyChanged(nameof(Genres));
            }
        }

        private ObservableCollection<Chapter> _chapters = new ObservableCollection<Chapter>();
        public ObservableCollection<Chapter> Chapters
        {
            get => _chapters;
            private set
            {
                _chapters = value;
                OnPropertyChanged(nameof(Chapters));
            }
        }

        private ObservableCollection<MangaCollection> _collections = new ObservableCollection<MangaCollection>();
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

        public MangaDetailPageVM(INavigationService navigationService, Manga selectedManga, List<Genre> genres, List<Teg> tegs, MangaScores? mangaScores, List<Publisher> publishers, DBConnection dbConnection)
        {
            _navigationService = navigationService;
            SelectedManga = selectedManga;
            MangaScores = mangaScores;

            Genres = new ObservableCollection<Genre>(genres);
            Tegs = new ObservableCollection<Teg>(tegs);
            Publishers = new ObservableCollection<Publisher>(publishers);
            _dbConnection = dbConnection;

            OpenScorePageCommand = new RelayCommand<object>(_ => ScoreManga());
            OpenMangaInfoPageCommand = new RelayCommand<object>(_ => OpenMangaInfo());
            OpenChaptersPageCommand = new RelayCommand<object>(_ => OpenChaptersPage());
            AddToCollectionCommand = new RelayCommand<object>(_ => AddMangaToCollection());

            LoadChapters();

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

                string? result = MangaCollectionRepository.UpdateMangasCollection(_dbConnection, userId, mangaId, collectionId);

                // Обработка результата
                if (result == "added")
                {
                    string message = $"Манга '{SelectedManga.Name}' была добавлена в коллекцию '{SelectedCollection.Title}'.";
                    AppServices.DialogService.ShowMessage(message, "Успех");
                }
                else if (result == "updated")
                {
                    string message = $"Манга '{SelectedManga.Name}' была обновлена в коллекции '{SelectedCollection.Title}'.";
                    AppServices.DialogService.ShowMessage(message, "Успех");
                }
                else
                {
                    // Логика для обработки ошибок
                    AppServices.DialogService.ShowMessage("Произошла ошибка при добавлении манги в коллекцию.", "Ошибка");
                }
            }
            else
            {
                // Логика для обработки случая, когда коллекция или манга не выбраны, или пользователь не авторизован
                AppServices.DialogService.ShowMessage("Пожалуйста, выберите коллекцию и мангу, а также убедитесь, что вы авторизованы.", "Ошибка");
            }
        }
        // Метод, реагирующий на изменение пользоватея
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
        // Загрузка списка коллекций
        private void LoadCollections()
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                Collections = new ObservableCollection<MangaCollection>();
                return;
            }
            var user = UserSession.Instance.CurrentUser;
            var collectionsList = MangaCollectionRepository.GetAllCollectionsByUser(_dbConnection, user.Id, user);
            Collections = new ObservableCollection<MangaCollection>(collectionsList);
        }
        // Открытие страницы с информацией о манге
        private void OpenMangaInfo()
        {
            var param = new MangaInfoPageParams(
                SelectedManga,
                Genres.ToList(),
                Tegs.ToList()
            );
            _navigationService.NavigateTo("MangaInfoPage", param);
        }
        // Открытие страницы с списком глав манги
        private void OpenChaptersPage()
        {
            var param = new ChaptersPageParams(
                Chapters.ToList()
            );
            _navigationService.NavigateTo("ChaptersPage", param);
        }
        // Открытие окна для оценки манги
        private void ScoreManga()
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                AppServices.DialogService.ShowMessage("Вы не авторизованы!", "Предупреждение");
            }
            else
            {
                AppServices.DialogService.ShowRateDialog(SelectedManga, _dbConnection);
            }
        }
        // Загрузка глав манги
        private void LoadChapters()
        {
            var allChapters = GetChaptersFromDatabase(); // Получаем главы из базы данных
            Chapters = new ObservableCollection<Chapter>(allChapters); // Обновляем коллекцию глав
        }
        private List<Chapter> GetChaptersFromDatabase()
        {
            return ChapterRepository.GetAllChapter(_dbConnection, SelectedManga.Id); // Получаем главы по ID манги
        }
    }
}
