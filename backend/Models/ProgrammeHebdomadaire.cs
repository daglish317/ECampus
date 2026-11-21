
namespace Backend.Models;

public class ProgrammeHebdomadaire
{
    public int Id { get; set; }

    public int AnneeAcademiqueId { get; set; }
    public AnneeAcademique? AnneeAcademique { get; set; }

    public string NomFichier { get; set; } = string.Empty;

    public DateTime DateImportation { get; set; }

    public int ClasseId { get; set; }

    public Classe? Classe { get; set; }

    // Exemple : "Importe", "EnPreparation", "Publie"
    public string Statut { get; set; } = "Importe";

    public DateOnly DateDebutSemaine { get; set; }

    public int NumeroVersion { get; set; } = 1;

    public int? ProgrammePrecedentId { get; set; }

    public ProgrammeHebdomadaire? ProgrammePrecedent { get; set; }

    public ProgrammeHebdomadaire? VersionSuivante { get; set; }

    public DateTime? DatePublication { get; set; }

    public ICollection<Seance> Seances { get; set; }
    = new List<Seance>();
}
