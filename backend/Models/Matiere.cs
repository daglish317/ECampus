namespace Backend.Models;

public class Matiere
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public ICollection<ProgrammeAcademique> Programmes { get; set; }
        = new List<ProgrammeAcademique>();
}