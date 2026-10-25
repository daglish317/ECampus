namespace Backend.Models;

public class Etudiant
{
    public int Id { get; set; }

    public int UtilisateurId { get; set; }

    public Utilisateur? Utilisateur { get; set; }
}