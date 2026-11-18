
namespace Backend.DTOs;

public class CreateProgrammeAcademiqueDto
{
    public int MatiereId { get; set; }

    public int NiveauId { get; set; }

    public int AnneeAcademiqueId { get; set; }

    public int SemestreId { get; set; }

    public DateTime DateEffet { get; set; }

    public DateTime? DateFinEffet { get; set; }
}
