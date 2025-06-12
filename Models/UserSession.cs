using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReadMangaApp.Models;

namespace ReadMangaApp.Models
{
    public class UserSession
    {
        private static UserSession? _instance;
        public static UserSession Instance => _instance ??= new UserSession();

        public event EventHandler<User>? UserChanged;

        private User? _currentUser;
        public User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                UserChanged?.Invoke(this, value!); // value может быть null, но Invoke сам по себе обрабатывает null event
            }
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}

