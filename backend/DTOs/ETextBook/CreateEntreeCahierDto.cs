using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.ETextBook;

public class CreateEntreeCahierDto
{

    [Required(ErrorMessage = "Le contenu enseigné est obligatoire.")] // RG09.2
    [MaxLength(2000)]
    public string ContenuEnseigne { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? ActivitesRealisees { get; set; } // RG09.3

    [MaxLength(2000)]
    public string? ActivitesPrevues { get; set; } // RG09.4 (Facultatif)
}