
namespace Backend.DTOs;

public class ProgrammeAcademiqueResponseDto
{
    public int Id { get; set; }

    public int MatiereId { get; set; }
    public string NomMatiere { get; set; } = string.Empty;

    public int NiveauId { get; set; }
    public string NomNiveau { get; set; } = string.Empty;

    public int AnneeAcademiqueId { get; set; }
    public string NomAnneeAcademique { get; set; } = string.Empty;

    public int SemestreId { get; set; }
    public string NomSemestre { get; set; } = string.Empty;

    public DateTime DateEffet { get; set; }
    public DateTime? DateFinEffet { get; set; }
}
