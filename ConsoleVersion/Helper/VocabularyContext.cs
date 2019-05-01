using System.Data.Entity;
using ConsoleVersion.Models;
using MySql.Data.Entity;

namespace ConsoleVersion.Helper
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class VocabularyContext : DbContext
    {
        public VocabularyContext() : base("DeveloperConnection")
        {}
        
        public DbSet<Vocabulary> Vocabularies { get; set; }
    }
}