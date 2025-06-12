using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Models
{
    public class User
    {
        private int _id;
        private string _username;
        private string _passwordHash;
        private string _email;
        private DateTime _createdAt;

        public User(int id, string username, string passwordHash, string email, DateTime createdAt)
        {
            _id = id;
            _username = username;
            _passwordHash = passwordHash;
            _email = email;
            _createdAt = createdAt;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }
    }
}
