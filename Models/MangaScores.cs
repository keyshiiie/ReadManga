using ReadMangaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMangaApp.Models
{
    public class MangaScores
    {
        private int _idManga;
        private decimal _score;

        public MangaScores(int idManga, decimal averageScore)
        {
            IdManga = idManga;
            AverageScore = averageScore;
        }

        public int IdManga
        {
            get => _idManga;
            set => _idManga = value;
        }
        public decimal AverageScore
        {
            get => _score;
            set => _score = value;
        }
    }
}
