using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

namespace ConsoleVersion
{
    public static class Operations
    {

        public static User Compare(UserContext userContext,User user)
        {
            User result = userContext.Users.Single(d=>d.Nickname==user.Nickname);
            
            if (result != null)
            {
                if (SecurePasswordHasher.Verify(user.Password, result.Password))
                    result = null;
            }
            return result;
        }
        public static void AddUser(UserContext userContext, User user)
        {
            user.Password = SecurePasswordHasher.Hash(user.Password);
            userContext.Users.Add(user);
            userContext.SaveChanges();
        }

        public static void UpdateUser(UserContext userContext, User changedUser)
        {
            userContext.Users.AddOrUpdate(changedUser);
            userContext.SaveChanges();
        }

        public static void RemoveUser(UserContext userContext, VocabularyContext vocabularyContext, User user)
        {
            //Think, how to remove all data's user in vocabulary
            userContext.Users.Remove(user);
            userContext.SaveChanges();
        }

        public static void AddVocabulary(VocabularyContext vocabularyContext, Vocabulary row)
        {
            vocabularyContext.Vocabularies.Add(row);
            vocabularyContext.SaveChanges();
        }

        public static void UpdateRow(VocabularyContext vocabularyContext, Vocabulary newRow)
        {
            vocabularyContext.Vocabularies.AddOrUpdate(newRow);
            vocabularyContext.SaveChanges();
        }

        public static void RemoveRow(VocabularyContext vocabularyContext, Vocabulary row)
        {
            vocabularyContext.Vocabularies.Remove(row);
            vocabularyContext.SaveChanges();
        }
    }
    public static class SecurePasswordHasher
    {
        /// <summary>
        /// Size of salt.
        /// </summary>
        private const int SaltSize = 16;
    
        /// <summary>
        /// Size of hash.
        /// </summary>
        private const int HashSize = 20;
    
        /// <summary>
        /// Creates a hash from a password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="iterations">Number of iterations.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password, int iterations)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
    
            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);
    
            // Combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
    
            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);
    
            // Format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }
    
        /// <summary>
        /// Creates a hash from a password with 10000 iterations
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }
    
        /// <summary>
        /// Checks if hash is supported.
        /// </summary>
        /// <param name="hashString">The hash.</param>
        /// <returns>Is supported?</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }
    
        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="hashedPassword">The hash.</param>
        /// <returns>Could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            // Check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }
    
            // Extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];
    
            // Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);
    
            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
    
            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
    
            // Get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}