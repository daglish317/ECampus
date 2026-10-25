namespace Backend.Models;

public class ProgrammeAcademique
{
    public int Id { get; set; }

    public int MatiereId { get; set; }

    public int NiveauId { get; set; }

    public int AnneeAcademiqueId { get; set; }

    public int SemestreId { get; set; }

    public Matiere? Matiere { get; set; }

    public Niveau? Niveau { get; set; }

    public AnneeAcademique? AnneeAcademique { get; set; }

    public Semestre? Semestre { get; set; }
}