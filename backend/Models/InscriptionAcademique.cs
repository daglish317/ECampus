
namespace Backend.Models;

public class InscriptionAcademique
{
    public int Id { get; set; }

    public int EtudiantId { get; set; }
    public Etudiant? Etudiant { get; set; }

    public int ClasseId { get; set; }
    public Classe? Classe { get; set; }

    public int AnneeAcademiqueId { get; set; }
    public AnneeAcademique? AnneeAcademique { get; set; }
}
