namespace OdontoApi.Models
{
    public class Procedure
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
    }
}