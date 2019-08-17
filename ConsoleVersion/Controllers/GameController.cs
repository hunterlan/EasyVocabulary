using ConsoleVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleVersion.Controllers
{
    class GameController
    {
        public static int points { get; set; }
        public static bool isTimerOver { get; set; }

        public GameController()
        {
            isTimerOver = false;
            points = 0;
        }

        public static Vocabulary ChooseRandomRow (List<Vocabulary> words, ref int randChoose)
        {
            int randomID, randomTrans;
            Random rand = new Random();

            randomID = rand.Next(words.Count);
            randomTrans = rand.Next(2);
            Vocabulary row = words[randomID];
            randChoose = randomTrans;

            return row;
        }

        private static bool CompareWords(string word1, string word2)
        {
            const int COUNT_MISTAKES = 1;
            int currentCount = 0;
            for (int i = 0; i < word1.Length && currentCount <= COUNT_MISTAKES; i++)
            {
                if (word2[i] != word1[i])
                    currentCount++;
            }
            if (currentCount > COUNT_MISTAKES)
                return false;
            return true;
        }

        public static bool Checker (Vocabulary row, string result, int codeLanguage)
        {
            result = result.ToLower();
            if(codeLanguage == 0)
            {
                row.LocalWord = row.LocalWord.ToLower();
                if (result.Length == row.LocalWord.Length)
                    return CompareWords(row.LocalWord, result);
                else
                    return false;
            }
            else
            {
                row.ForeignWord = row.ForeignWord.ToLower();
                if (result.Length == row.ForeignWord.Length)
                    return CompareWords(row.ForeignWord, result);
                else
                    return false;
            }
        }

        public static void TimerOver(object obj)
        {
            isTimerOver = true;
        }
    }
}
