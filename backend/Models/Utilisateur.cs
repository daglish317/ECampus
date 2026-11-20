using Backend.Models.ETextBook;

namespace Backend.Models;

public class Utilisateur
{
    public int Id { get; set; }

    // Identifiant de l'utilisateur chez Clerk
    public string ClerkUserId { get; set; } = string.Empty;

    public string Nom { get; set; } = string.Empty;

    public string Prenom { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    // Rôle applicatif dans eCampus
    public string Role { get; set; } = string.Empty;

    public ICollection<DecisionCahier> DecisionsCahier { get; set; } = new List<DecisionCahier>();
}