using System.ComponentModel;

namespace ReadMangaApp.Models
{
    public class MangaCollection : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private bool _isDefault;
        private User _user;

        public MangaCollection(int id, string name, User user)
        {
            _id = id;
            _title = name;
            _user = user;
        }


        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }
        public User User
        {
            get => _user;
            set => _user = value;   
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
