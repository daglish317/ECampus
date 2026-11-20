using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.ETextBook;

public class UpdateEntreeCahierDto
{
    [Required(ErrorMessage = "Le contenu enseigné est obligatoire.")]
    [MaxLength(2000)]
    public string ContenuEnseigne { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? ActivitesRealisees { get; set; }

    [MaxLength(2000)]
    public string? ActivitesPrevues { get; set; }
}