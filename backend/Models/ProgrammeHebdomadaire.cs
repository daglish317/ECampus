
namespace Backend.Models;

public class ProgrammeHebdomadaire
{
    public int Id { get; set; }

    public int AnneeAcademiqueId { get; set; }
    public AnneeAcademique? AnneeAcademique { get; set; }

    public string NomFichier { get; set; } = string.Empty;

    public DateTime DateImportation { get; set; }

    // Exemple : "Importe", "EnPreparation", "Publie"
    public string Statut { get; set; } = "Importe";
}
