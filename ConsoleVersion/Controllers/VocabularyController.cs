using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

namespace ConsoleVersion
{
    public static class VocabularyController
    {
        public static void AddRow(VocabularyContext vocabularyContext, Vocabulary row)
        {
            try
            {
                vocabularyContext.Vocabularies.Add(row);
                vocabularyContext.SaveChanges();
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            } 
        }
        public static Vocabulary createRow(User currentUser)
        {
            Vocabulary row = new Vocabulary();
            string foreignWord, localWord, transcription;
            bool go = false;

            do
            {
                Console.WriteLine("Write foreign word.");
                foreignWord = Console.ReadLine();
                if (foreignWord != null)
                    go = true;
                else
                    Console.WriteLine("Line is empty.");
            } while (!go);
            go = false;

            Console.WriteLine("Write transcription (optionally)");
            transcription = Console.ReadLine();

            do
            {
                Console.WriteLine("Write local word.");
                localWord = Console.ReadLine();
                if (localWord != null)
                    go = true;
                else
                    Console.WriteLine("Line is empty.");
            } while (!go);

            row.UserID = currentUser.Id;
            row.ForeignWord = foreignWord;
            row.Transcription = transcription;
            row.LocalWord = localWord;

            return row;
        }

        public static void UpdateRow(VocabularyContext vocabularyContext, Vocabulary newRow)
        {
            try
            {
                vocabularyContext.Vocabularies.AddOrUpdate(newRow);
                vocabularyContext.SaveChanges();
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            }
        }

        public static void RemoveRow(VocabularyContext vocabularyContext, Vocabulary row)
        {
            try
            {
                vocabularyContext.Vocabularies.Remove(row);
                vocabularyContext.SaveChanges();
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            }
        }

        public static Vocabulary FindRow(string key, byte foundChoose, VocabularyContext vocabularyContext)
        {
            Vocabulary foundRow = new Vocabulary();

            switch (foundChoose)
            {
                case 1:
                    foundRow = vocabularyContext.Vocabularies.Single(d => d.ForeignWord == key);
                    break;
                case 2:
                    foundRow = vocabularyContext.Vocabularies.Single(d => d.Transcription == key);
                    break;
                case 3:
                    foundRow = vocabularyContext.Vocabularies.Single(d => d.LocalWord == key);
                    break;
            }

            return foundRow;
        }

        public static void RemoveVocabulary(VocabularyContext vocabularyContext, User user)
        {
            try
            {
                var rows = vocabularyContext.Vocabularies.ToList();
                foreach (var row in rows)
                {
                    if (row.UserID == user.Id)
                        vocabularyContext.Vocabularies.Remove(row);
                }
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            }
        }
    }
    
}