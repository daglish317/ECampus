namespace Backend.Models;

public class AnneeAcademique
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public DateOnly DateDebut { get; set; }

    public DateOnly DateFin { get; set; }

    public bool EstActive { get; set; }
}

