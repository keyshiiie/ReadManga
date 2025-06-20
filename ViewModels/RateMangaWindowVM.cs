using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class RateMangaWindowVM : ViewModelBase
    {
        private readonly MangaScoreApiClient _mangaScoreApiClient;
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

        public RateMangaWindowVM(Manga selectedManga, MangaScoreApiClient mangaScoreApiClient)
        {
            _mangaScoreApiClient = mangaScoreApiClient;
            _selectedManga = selectedManga;
            SubmitCommand = new RelayCommand<object>(async _ => await SubmitRateAsync());
            CancelCommand = new RelayCommand<object>(_ => RequestClose?.Invoke());
        }

        private async Task SubmitRateAsync()
        {
            var user = UserSession.Instance.CurrentUser;
            if (user == null)
            {
                AppServices.DialogService.ShowMessage("Вы не авторизованы!", "Ошибка");
                return;
            }

            try
            {
                // Предполагается, что у API есть метод для обновления оценки, например POST с параметрами userId, mangaId и score
                await _mangaScoreApiClient.SubmitScoreAsync(user.Id, _selectedManga.Id, _currentScore);

                AppServices.DialogService.ShowMessage("Оценка добавлена!");
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                AppServices.DialogService.ShowMessage($"Ошибка при отправке оценки: {ex.Message}", "Ошибка");
            }
        }
    }
}