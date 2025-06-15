using System.ComponentModel;

namespace ReadMangaApp.Models
{
    public class MangaCollection : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        private bool _isDefault;
        public User User { get; set; }

        public MangaCollection(int id, string name, User user)
        {
            Id = id;
            Title = name;
            User = user;
        }
        public bool IsDefault
        {
            get => _isDefault;
            set
            {
                if (_isDefault != value)
                {
                    _isDefault = value;
                    OnPropertyChanged(nameof(_isDefault));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
