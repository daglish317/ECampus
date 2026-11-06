namespace Backend.Models;

public class Niveau
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<Classe> Classes { get; set; }
        = new List<Classe>();
}