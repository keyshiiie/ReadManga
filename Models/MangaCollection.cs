using System.ComponentModel;

namespace ReadMangaApp.Models
{
    public class MangaCollection : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        private bool _isDefault;
        public required User User { get; set; }

        public MangaCollection(){}

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
