namespace PortfolioApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        // Simpan password hash, bukan plain text
        public string PasswordHash { get; set; } = null!;
    }
}
