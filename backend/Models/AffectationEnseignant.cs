
namespace Backend.Models;

public class AffectationEnseignant
{
    public int Id { get; set; }

    public int EnseignantId { get; set; }
    public Enseignant? Enseignant { get; set; }

    public int ProgrammeAcademiqueId { get; set; }
    public ProgrammeAcademique? ProgrammeAcademique { get; set; }

    public DateOnly DateDebut { get; set; }
    public DateOnly? DateFin { get; set; }
}
