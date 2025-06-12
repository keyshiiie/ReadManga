using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Repository;
using ReadMangaApp.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class RateMangaPageVM : INotifyPropertyChanged
    {
        private RateMangaPage _rateMangaPage;
        private readonly DBConnection _dbConnection; // Добавляем поле для подключения к БД

        private int _currentScore = 1; // Измените на int
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

        public RateMangaPageVM(DBConnection dbConnection, Manga selectedManga, RateMangaPage rateMangaPage)
        {
            _selectedManga = selectedManga;
            _rateMangaPage = rateMangaPage;
            _dbConnection = dbConnection; // Сохраняем подключение к БД
            SubmitCommand = new RelayCommand<object>(_ => SubmitRate());
            CancelCommand = new RelayCommand<object>(_ => { /* закрыть окно */ });
        }

        private void SubmitRate()
        {
            var user = UserSession.Instance.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MangaScoreRepository.UpdateScore(_dbConnection, user.Id, _selectedManga.Id, CurrentScore);
            if (result == "added")
            {
                MessageBox.Show("Оценка добавлена!");
            }
            else if (result == "updated")
            {
                MessageBox.Show("Оценка обновлена!");
            }
            else
            {
                MessageBox.Show("Оценка сохранена!");
            }
            _rateMangaPage.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
