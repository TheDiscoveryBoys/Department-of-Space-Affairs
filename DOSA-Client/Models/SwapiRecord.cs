namespace DOSA_Client.Models
{
    public class SwapiResponse
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<SwapiRecord> Results { get; set; }
    }

    public record SwapiRecord(string Name);
}
