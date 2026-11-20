using Backend.Models.ETextBook;

namespace Backend.Models;

public class Enseignant
{
    public int Id { get; set; }

    public int UtilisateurId { get; set; }

    public Utilisateur? Utilisateur { get; set; }

    public ICollection<EntreeCahier> EntreesCahier { get; set; } = new List<EntreeCahier>();
}
