
namespace Backend.DTOs;

public class ModifierSemestreDto
{
    public string Nom { get; set; } = string.Empty;

    public DateOnly DateDebut { get; set; }

    public DateOnly DateFin { get; set; }
}
