
namespace Backend.DTOs;

public class CreerSemestreDto
{
    public string Nom { get; set; } = string.Empty;

    public DateOnly DateDebut { get; set; }

    public DateOnly DateFin { get; set; }

    public int AnneeAcademiqueId { get; set; }
}
