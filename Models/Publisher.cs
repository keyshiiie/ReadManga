using System.ComponentModel;

namespace ReadMangaApp.Models
{
    public class Publisher : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool _isSelected;
        public Publisher(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
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