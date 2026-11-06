namespace Backend.Models;

public class Semestre
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public DateOnly DateDebut { get; set; }

    public DateOnly DateFin { get; set; }

    public int AnneeAcademiqueId { get; set; }

    public AnneeAcademique? AnneeAcademique { get; set; }
}