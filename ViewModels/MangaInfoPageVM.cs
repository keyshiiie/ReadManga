using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BeautyShop.Commands;
using ReadMangaApp.Services;
using System.Windows.Navigation;
using System.Data.Common;

namespace ReadMangaApp.ViewModels
{
    class MangaInfoPageVM : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public Manga SelectedManga { get; }
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
        public ICommand SortMangaGenreCommand { get; }
        public ICommand SortMangaTegCommand { get; }

        public MangaInfoPageVM(INavigationService mainNavigationService, Manga selectedManga)
        {
            _navigationService = mainNavigationService;
            SelectedManga = selectedManga;
            Genres = new ObservableCollection<Genre>(selectedManga.Genres ?? new List<Genre>());
            Tegs = new ObservableCollection<Teg>(selectedManga.Tegs ?? new List<Teg>());

            SortMangaGenreCommand = new RelayCommand<Genre>(genre => SortMangaByGenre(genre));
            SortMangaTegCommand = new RelayCommand<Teg>(teg => SortMangaByTeg(teg));
        }

        private void SortMangaByGenre(Genre selectedGenre)
        {
            AppServices.DialogService.ShowMessage("Выбран жанр: " + selectedGenre.Name);
            _navigationService.NavigateTo("MainMangaPage", selectedGenre);
        }

        private void SortMangaByTeg(Teg selectedTeg)
        {
            AppServices.DialogService.ShowMessage("Выбран тег: " + selectedTeg.Name);
            _navigationService.NavigateTo("MainMangaPage", selectedTeg);
        }
    }
}