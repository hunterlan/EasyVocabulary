using System.Data.Entity;

using ConsoleVersion.Models;
using MySql.Data.Entity;

namespace ConsoleVersion.Helper
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class UserContext : DbContext
    {
        public UserContext() : base("DeveloperConnection")
        { }
        
        public DbSet<User> Users { get; set; }
    }
}