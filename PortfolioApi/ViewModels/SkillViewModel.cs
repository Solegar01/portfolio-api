public class SkillViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Level { get; set; }  // misalnya: 1–5, atau 1–100
    public string Category { get; set; } = null!; // optional: e.g., Frontend, Backend, DevOps
}