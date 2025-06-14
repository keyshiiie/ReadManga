using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Repository;
using ReadMangaApp.Services;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class RateMangaPageVM : ViewModelBase
    {
        private readonly DBConnection _dbConnection; 

        public event Action? RequestClose;


        private int _currentScore = 1;
        private Manga _selectedManga;

        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                if (_currentScore != value)
                {
                    _currentScore = value;
                    OnPropertyChanged(nameof(CurrentScore));
                }
            }
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public RateMangaPageVM(DBConnection dbConnection, Manga selectedManga)
        {
            _selectedManga = selectedManga;
            _dbConnection = dbConnection; // Сохраняем подключение к БД
            SubmitCommand = new RelayCommand<object>(_ => SubmitRate());
            CancelCommand = new RelayCommand<object>(_ => RequestClose?.Invoke());
        }

        private void SubmitRate()
        {
            var user = UserSession.Instance.CurrentUser;
            if (user == null)
            {
                AppServices.DialogService.ShowMessage("Вы не авторизованы!", "Ошибка");
                return;
            }
            var result = MangaScoreRepository.UpdateScore(_dbConnection, user.Id, _selectedManga.Id, CurrentScore);
            AppServices.DialogService.ShowMessage("Оценка добавлена!");
            RequestClose?.Invoke();
        }
    }
}
