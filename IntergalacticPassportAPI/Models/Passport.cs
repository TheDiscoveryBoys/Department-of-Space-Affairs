namespace IntergalacticPassportAPI.Models
{
    public class Passport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Planet { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}