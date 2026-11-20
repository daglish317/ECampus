namespace Backend.Models.ETextBook;

public class CahierTexte
{
    public int Id { get; set; }

    // Lien vers le ProgrammeAcademique (OffreMatiere)
    public int ProgrammeAcademiqueId { get; set; }
    public ProgrammeAcademique? ProgrammeAcademique { get; set; }

    public DateTime DateCreation { get; set; }

    public bool Actif { get; set; } = true;

    // Navigation vers les entrées
    public ICollection<EntreeCahier> Entrees { get; set; } = new List<EntreeCahier>();
}