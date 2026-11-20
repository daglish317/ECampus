namespace Backend.Models.ETextBook;

public class DecisionCahier
{
    public int Id { get; set; }

    public int EntreeCahierId { get; set; }
    public EntreeCahier? EntreeCahier { get; set; }

    // Le Responsable Pédagogique qui prend la décision
    public int UtilisateurId { get; set; }
    public Utilisateur? Utilisateur { get; set; }

    public TypeDecisionCahierEnum Decision { get; set; }

    // RG10.3 : Obligatoire si RETOUR_CORRECTION
    public string? Commentaire { get; set; }

    public DateTime DateDecision { get; set; }
}