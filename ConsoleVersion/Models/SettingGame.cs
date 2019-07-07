using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Models
{
    class SettingGame
    {
        public byte countWords { get; set; } //2, 4, 6, 8, 10

        public bool isIncludeToTable { get; set; }

        public readonly int TIME = 30;
    }
}