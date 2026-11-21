using Backend.Models.ETextBook;

namespace Backend.Models;

public class Seance
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly HeureDebut { get; set; }

    public int DureeMinutes { get; set; }

    public string Salle { get; set; } = string.Empty;

    public int AffectationEnseignantId { get; set; }
    public AffectationEnseignant? AffectationEnseignant { get; set; }

    public int ProgrammeHebdomadaireId { get; set; }

    public ProgrammeHebdomadaire? ProgrammeHebdomadaire { get; set; }

    public EntreeCahier? EntreeCahier { get; set; }
}
