namespace ConsoleVersion.Models
{
    public class Vocabulary
    {
        public int Id { get; set; }
        
        public string ForeignWord { get; set; }
        
        public string Transcription { get; set; }
        
        public string LocalWord { get; set; }
        
        public int UserID { get; set; }
    }
}