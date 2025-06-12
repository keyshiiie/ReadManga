using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Models
{
    public class StatusTranslation
    {
        private int _id;
        private string _name;

        public StatusTranslation(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }
    }
}
