using ReadMangaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ReadMangaApp.Models
{
    public class Manga
    {
        private int _id;
        private string _name;
        private int _datePublished;
        private string _coverUrl;
        private StatusReleased _statusReleased;
        private StatusTranslation _statusTranslation;
        private List<Teg> _tegs;
        private List<Genre> _genres;
        private string? _author;
        private string? _description;
        private string? _alternativeTitle;
        private List<Publisher> _publisher;
        private TypeManga _type;
        private MangaScores _score;

        public Manga(int id, string name, int datePublished, string coverUrl, StatusReleased statusReleased, StatusTranslation statusTranslation, TypeManga type, string? author, string? description, string? alternativeTitle)
        {
            _id = id;
            _name = name;
            _datePublished = datePublished;
            _coverUrl = coverUrl;
            _statusReleased = statusReleased;
            _statusTranslation = statusTranslation;
            _tegs = new List<Teg>();
            _genres = new List<Genre>();
            _type = type;
            _author = author;
            _description = description;
            _publisher = new List<Publisher>();
            _alternativeTitle = alternativeTitle;
            _score = new MangaScores(id, 0); // Инициализация MangaScores с ID и начальным значением 0
        }

        public int Id => _id; // только для чтения

        public string Name
        {
            get => _name;
            private set => _name = value; // можно изменять только внутри класса
        }

        public string? Author
        {
            get => _author;
            private set => _author = value;
        }

        public string? Description
        {
            get => _description;
            private set => _description = value;
        }

        public string? AlternativeTitle
        {
            get => _alternativeTitle;
            private set => _alternativeTitle = value;
        }

        public int DatePublished => _datePublished; // только для чтения

        public string CoverUrl
        {
            get => _coverUrl;
            private set => _coverUrl = value;
        }

        public StatusReleased StatusReleased => _statusReleased; // только для чтения

        public StatusTranslation StatusTranslation => _statusTranslation; // только для чтения

        public TypeManga TypeManga => _type; // только для чтения

        public List<Teg> Tegs
        {
            get => _tegs;
            set => _tegs = value; // Теперь можно изменять и из других классов
        }

        public List<Genre> Genres
        {
            get => _genres;
            set => _genres = value; // Теперь можно изменять и из других классов
        }

        public MangaScores MangaScores
        {
            get => _score;
            set => _score = value; // Теперь можно изменять и из других классов
        }

        public decimal AverageScore
        {
            get => MangaScores != null ? MangaScores.AverageScore : 0.0m; // Используйте 0.0m для decimal
        }

        public Brush RatingBackground
        {
            get
            {
                if (AverageScore >= 0 && AverageScore < 4)
                {
                    return Brushes.Red; // Красный фон
                }
                else if (AverageScore >= 4 && AverageScore < 7)
                {
                    return Brushes.Gold; // Желтый фон
                }
                else if (AverageScore >= 7 && AverageScore <= 10)
                {
                    return Brushes.Green; // Зеленый фон
                }
                return Brushes.Transparent; // Прозрачный фон по умолчанию
            }
        }
    }

}
