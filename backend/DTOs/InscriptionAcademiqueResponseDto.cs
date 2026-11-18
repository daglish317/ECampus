
namespace Backend.DTOs;

public class InscriptionAcademiqueResponseDto
{
    public int Id { get; set; }

    public int EtudiantId { get; set; }

    public string NomEtudiant { get; set; } = string.Empty;

    public string PrenomEtudiant { get; set; } = string.Empty;

    public string EmailEtudiant { get; set; } = string.Empty;

    public int ClasseId { get; set; }

    public string NomClasse { get; set; } = string.Empty;

    public int AnneeAcademiqueId { get; set; }

    public string NomAnneeAcademique { get; set; } = string.Empty;
}
