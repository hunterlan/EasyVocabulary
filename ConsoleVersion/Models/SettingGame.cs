using ConsoleVersion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Models
{
    public class SettingGame
    {
        //less 40 words - only 2 points, less 60 words, 4 point and etc
        public byte countPoints { get; set; } //2, 4, 6, 8, 10
        public readonly string[] TYPE_GAME;
        public bool isIncludeToTable { get; set; }

        //numbers is count of words
        private const int FIRST_LEVEL = 40;
        private const int SECOND_LEVEL = 60;
        private const int THIRD_LEVEL = 80;
        private const int FOURTH_LEVEL = 100;
        private const int COUNT_GAMES = 3;

        public SettingGame(VocabularyContext vocabulary)
        {
            TYPE_GAME = new string[COUNT_GAMES]
            { "Write translation", "One from 4th", "Matching translation"};
            isIncludeToTable = true;
            var listCount = vocabulary.Vocabularies.ToList().Count;
            if (listCount < FIRST_LEVEL)
                countPoints = 2;
            else if (listCount < SECOND_LEVEL)
                countPoints = 4;
            else if (listCount < THIRD_LEVEL)
                countPoints = 6;
            else if (listCount < FOURTH_LEVEL)
                countPoints = 8;
            else
                countPoints = 10;
        }

        public static readonly int TIME = 30;
    }
}