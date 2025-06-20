using ReadMangaApp.Models;
using BeautyShop.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReadMangaApp.Services;
using ReadMangaApp.Dtos;
using System.Data.Common;
using ReadMangaApp.DataAccess;
using System.Data;
using System.Net.Http;

namespace ReadMangaApp.ViewModels
{
    public class MangaDetailPageVM : ViewModelBase
    {
        private readonly PageApiClient _pageApiClient;
        private readonly ChapterApiClient _chapterApiClient;
        private readonly MangaCollectionApiClient _mangaCollectionApiClient;
        private readonly MangaScoreApiClient _mangaScoreApiClient;
        private readonly INavigationService _navigationService;
        public Manga SelectedManga { get; }

        private ObservableCollection<Publisher> _publishers = new ObservableCollection<Publisher>();
        public ObservableCollection<Publisher> Publishers
        {
            get => _publishers;
            private set
            {
                _publishers = value;
                OnPropertyChanged(nameof(Publishers));
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

        public MangaDetailPageVM(INavigationService navigationService, Manga selectedManga, MangaScoreApiClient mangaScoreApiClient, MangaCollectionApiClient mangaCollectionApiClient, ChapterApiClient chapterApiClient, PageApiClient pageApiClient)
        {

            _pageApiClient = pageApiClient;
            _chapterApiClient = chapterApiClient;
            _mangaCollectionApiClient = mangaCollectionApiClient;
            _mangaScoreApiClient = mangaScoreApiClient;
            _navigationService = navigationService;
            SelectedManga = selectedManga;

            MangaScores = selectedManga.MangaScores;
            Genres = new ObservableCollection<Genre>(selectedManga.Genres ?? new List<Genre>());
            Tegs = new ObservableCollection<Teg>(selectedManga.Tegs ?? new List<Teg>());
            Publishers = new ObservableCollection<Publisher>(selectedManga.Publishers ?? new List<Publisher>());
            

            OpenScorePageCommand = new RelayCommand<object>(_ => ScoreManga());
            OpenMangaInfoPageCommand = new RelayCommand<object>(_ => OpenMangaInfo());
            OpenChaptersPageCommand = new RelayCommand<object>(_ => OpenChaptersPage());
            AddToCollectionCommand = new RelayCommand<object>(_ => AddMangaToCollection());

            LoadChapters(selectedManga);

            // Подписка на событие изменения пользователя
            UserSession.Instance.UserChanged += OnUserChanged;
            if (UserSession.Instance.CurrentUser != null)
            {
                LoadCollections();
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
        private async void LoadCollections()
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                Collections = new ObservableCollection<MangaCollection>();
                return;
            }

            var user = UserSession.Instance.CurrentUser;
            var collectionsList = await _mangaCollectionApiClient.GetAllCollectionsByUserAsync(user.Id);
            Collections = new ObservableCollection<MangaCollection>(collectionsList);

            if (!string.IsNullOrEmpty(SelectedManga.Collection))
            {
                var matchingCollection = Collections.FirstOrDefault(c => c.Title == SelectedManga.Collection);
                if (matchingCollection != null)
                {
                    SelectedCollection = matchingCollection;
                    SelectedCollectionId = matchingCollection.Id;
                }
            }
        }

        private async void AddMangaToCollection()
        {
            if (SelectedCollection == null)
            {
                AppServices.DialogService.ShowMessage("Пожалуйста, выберите коллекцию.", "Информация");
                return;
            }

            try
            {
                await _mangaCollectionApiClient.UpdateMangasCollectionAsync(SelectedManga.Id, SelectedCollection.Id);
                CollectionChangedNotifier.NotifyCollectionsChanged();
                AppServices.DialogService.ShowMessage("Манга успешно добавлена в коллекцию.", "Успех");
            }
            catch (Exception ex)
            {
                AppServices.DialogService.ShowMessage($"Ошибка при добавлении манги в коллекцию: {ex.Message}", "Ошибка");
            }
        }


        // Открытие страницы с информацией о манге
        private void OpenMangaInfo()
        {
            _navigationService.NavigateTo("MangaInfoPage", SelectedManga);
        }
        // Открытие страницы с списком глав манги
        private void OpenChaptersPage()
        {
            var param = new ChaptersPageParams(
                Chapters.ToList(),
                _pageApiClient
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
               AppServices.DialogService.ShowRateDialog(SelectedManga, _mangaScoreApiClient);
            }
        }
        // Загрузка глав манги
        private async void LoadChapters(Manga selectedManga)
        {
            var chapters = await _chapterApiClient.GetAllChaptersAsync(selectedManga.Id);

            if (chapters == null || !chapters.Any())
            {
                AppServices.DialogService.ShowMessage("У данной манги пока нет глав.", "Информация");
                Chapters.Clear();
                return;
            }

            Chapters.Clear();
            foreach (var chapter in chapters)
            {
                Chapters.Add(chapter);
            }
        }
    }
}