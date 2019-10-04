using System;

namespace ConsoleVersion.Models
{
    [Serializable]
    public class User
    {
        public int Id { get; set; }
        
        public string Nickname { get; set; }
        
        public string Password { get; set; }
        
        public string Email { get; set; }   
    }
}