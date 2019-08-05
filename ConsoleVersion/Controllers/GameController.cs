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
        public int points { get; set; }
        private static string generateWord (List<Vocabulary> words)
        {
            int randomID, randomTrans;
            Random rand = new Random();
            randomID = rand.Next(words.Count);
            randomTrans = rand.Next(2);
            Vocabulary row = words[randomID];

            if (randomTrans == 0)
                return row.ForeignWord;
            else
                return row.LocalWord;
        }
        public static void ConsoleWriteTranscription(List<Vocabulary> words)
        {
            const int TIMER_MILISECONDS = 30000;
            string startWord;

            //do
            //{
            //    startWord = generateWord(words);
            //TODO: Write the timer. Timer ticking while player write the word. When we have got  the word, timer stopped, and wait for new word
            //}while()
        }
    }
}
