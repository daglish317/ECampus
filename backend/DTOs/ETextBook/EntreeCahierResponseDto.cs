using Backend.Models.ETextBook;

namespace Backend.DTOs.ETextBook;

public class EntreeCahierResponseDto
{
    // IDs utiles pour les appels API suivants du Frontend
    public int Id { get; set; }
    public int SeanceId { get; set; }
    public int CahierTexteId { get; set; }

    // Informations de contexte pour l'affichage
    public string NomMatiere { get; set; } = string.Empty;
    public string NomEnseignant { get; set; } = string.Empty;
    public DateTime DateSeance { get; set; }
    public string Salle { get; set; } = string.Empty;
    public int DureeMinutes { get; set; } // Utile pour EF08 (calcul des heures)

    // Contenu rempli par l'enseignant
    public string ContenuEnseigne { get; set; } = string.Empty;
    public string? ActivitesRealisees { get; set; }
    public string? ActivitesPrevues { get; set; }

    // Statut et traçabilité (RG09.6, RG09.7)
    public StatutCahier Statut { get; set; }
    public DateTime? DateSoumission { get; set; }

    // RG10.5 : Le commentaire de la dernière décision doit être accessible à l'enseignant
    public string? DernierCommentaire { get; set; }

    // RG10.7 : Historique des décisions
    public List<DecisionCahierResponseDto> HistoriqueDecisions { get; set; } = new();
}

public class DecisionCahierResponseDto
{
    public TypeDecisionCahierEnum Decision { get; set; }
    public string? Commentaire { get; set; }
    public string NomValidateur { get; set; } = string.Empty;
    public DateTime DateDecision { get; set; }
}