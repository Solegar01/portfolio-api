namespace PortfolioApi.Models
{
    public class Education
    {
        public int Id { get; set; }
        public string Institution { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? FieldOfStudy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
