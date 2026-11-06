namespace Backend.Models;

public class Classe
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public int NiveauId { get; set; }

    public Niveau? Niveau { get; set; }

    public ICollection<InscriptionAcademique> Inscriptions { get; set; }
        = new List<InscriptionAcademique>();
}