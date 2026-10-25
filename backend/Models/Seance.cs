
namespace Backend.Models;

public class Seance
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly HeureDebut { get; set; }

    public int DureeMinutes { get; set; }

    public string Salle { get; set; } = string.Empty;

    public int ProgrammeAcademiqueId { get; set; }
    public ProgrammeAcademique? ProgrammeAcademique { get; set; }

    public int AffectationEnseignantId { get; set; }
    public AffectationEnseignant? AffectationEnseignant { get; set; }

    public int? ProgrammeHebdomadaireId { get; set; }

    public ProgrammeHebdomadaire? ProgrammeHebdomadaire { get; set; }
}
