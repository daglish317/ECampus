using System.ComponentModel.DataAnnotations;
using Backend.Models.ETextBook;

namespace Backend.DTOs.ETextBook;

public class DecisionCahierDto
{
    

    [Required(ErrorMessage = "Le type de décision est obligatoire.")]
    public TypeDecisionCahierEnum Decision { get; set; }

    // RG10.3 : Obligatoire si RETOUR_CORRECTION (validé dans le Service)
    // RG10.4 : Le système interdit le retour si le commentaire est vide (validé dans le Service)
    [MaxLength(2000)]
    public string? Commentaire { get; set; }
}