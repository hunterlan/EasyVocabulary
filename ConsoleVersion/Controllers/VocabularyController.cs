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
            Vocabulary foundRow = null;

            foreach (var vocabulary in vocabularyContext.Vocabularies.ToList())
            {
                switch (foundChoose)
                {
                    case 1:
                        if (vocabulary.ForeignWord == key)
                            foundRow = vocabulary;
                        break;
                    case 2:
                        if (vocabulary.Transcription == key)
                            foundRow = vocabulary;
                        break;
                    case 3:
                        if (vocabulary.LocalWord == key)
                            foundRow = vocabulary;
                        break;
                }
            }
            return foundRow;
        }

        public static Vocabulary FindRowByID(int id, VocabularyContext vocabularyContext)
        {
            return vocabularyContext.Vocabularies.First(d => d.Id == id);
        }

        public static Vocabulary FindRow(Vocabulary row, VocabularyContext vocabularyContext)
        {
            foreach (var vocabulary in vocabularyContext.Vocabularies.ToList())
            {
                if (vocabulary.ForeignWord == row.ForeignWord &&
                    vocabulary.Transcription == row.Transcription &&
                    vocabulary.LocalWord == row.LocalWord &&
                    vocabulary.UserID == row.UserID)
                    return vocabulary;
            }
            return null;
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