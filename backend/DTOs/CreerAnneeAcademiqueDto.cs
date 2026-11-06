
namespace Backend.DTOs;

public class CreerAnneeAcademiqueDto
{
    public string Nom { get; set; } = string.Empty;

    public DateOnly DateDebut { get; set; }

    public DateOnly DateFin { get; set; }
}
