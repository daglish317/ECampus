namespace Backend.Models.ETextBook;

public class EntreeCahier
{
    public int Id { get; set; }

    public int CahierTexteId { get; set; }
    public CahierTexte? CahierTexte { get; set; }

    public int SeanceId { get; set; }
    public Seance? Seance { get; set; }

    // L'enseignant qui remplit l'entrée
    public int EnseignantId { get; set; }
    public Enseignant? Enseignant { get; set; }

    // RG09.2 : Le contenu enseigné est obligatoire
    public string ContenuEnseigne { get; set; } = string.Empty;
    
    public string? ActivitesRealisees { get; set; }
    public string? ActivitesPrevues { get; set; }

    public DateTime? DateSoumission { get; set; }

    public StatutCahier Statut { get; set; } = StatutCahier.EN_PREPARATION;

    // Historique des décisions (EF10)
    public ICollection<DecisionCahier> Decisions { get; set; } = new List<DecisionCahier>();
}