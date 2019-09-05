using ConsoleVersion.Models;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Helper
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class TournTableContext : DbContext
    {
        public TournTableContext() : base("DeveloperConnection")
        { }

        public DbSet<RecordTournTable> Records { get; set; }
    }
}
