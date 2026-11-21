using Backend.DTOs.ETextBook;

namespace Backend.Services.ETextBook;

public interface ICahierTexteService
{
    // EF07 : Créer automatiquement un cahier de texte (RG07.1)
    Task CreerCahierTextePourProgrammeAsync(int programmeAcademiqueId);

    // EF09 : Récupérer une entrée de cahier pour une séance
    Task<EntreeCahierResponseDto?> ObtenirEntreeCahierParSeanceAsync(int seanceId);

    // EF09 : Soumettre une entrée de cahier (RG09.1 à RG09.9)
    Task<EntreeCahierResponseDto> SoumettreEntreeCahierAsync(
        int seanceId, 
        int enseignantId, 
        CreateEntreeCahierDto dto);

    // EF09 : Corriger une entrée retournée (RG10.6)
    Task<EntreeCahierResponseDto> CorrigerEntreeCahierAsync(
        int entreeCahierId, 
        int enseignantId, 
        UpdateEntreeCahierDto dto);

    // EF10 : Valider ou retourner une entrée (RG10.1 à RG10.7)
    Task<EntreeCahierResponseDto> TraiterDecisionAsync(
        int entreeCahierId, 
        int utilisateurId, 
        DecisionCahierDto dto);

    // EF08 : Calculer les heures réalisées pour un programme
    Task<double> CalculerHeuresRealiseesAsync(int programmeAcademiqueId);
}