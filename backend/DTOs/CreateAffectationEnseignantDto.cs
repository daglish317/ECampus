
namespace Backend.DTOs;

public class CreateAffectationEnseignantDto
{
    public int EnseignantId { get; set; }

    public int ProgrammeAcademiqueId { get; set; }

    public DateOnly DateDebut { get; set; }

    public DateOnly? DateFin { get; set; }
}
