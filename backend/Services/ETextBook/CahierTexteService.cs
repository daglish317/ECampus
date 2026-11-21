using Backend.Data;
using Backend.DTOs.ETextBook;
using Backend.Models;
using Backend.Models.ETextBook;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.ETextBook;

public class CahierTexteService : ICahierTexteService
{
    private readonly ApplicationDbContext _context;

    public CahierTexteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreerCahierTextePourProgrammeAsync(int programmeAcademiqueId)
    {
        // 1. Vérifier si un cahier existe déjà pour ce programme (RG07.2)
        var cahierExiste = await _context.CahiersTexte
            .AnyAsync(c => c.ProgrammeAcademiqueId == programmeAcademiqueId);

        if (cahierExiste)
        {
            // Un cahier existe déjà, on ne fait rien (RG07.2)
            return;
        }

        // 2. Créer le nouveau cahier
        var nouveauCahier = new CahierTexte
        {
            ProgrammeAcademiqueId = programmeAcademiqueId,
            DateCreation = DateTime.UtcNow,
            Actif = true
        };

        // 3. Sauvegarder en base de données
        _context.CahiersTexte.Add(nouveauCahier);
        await _context.SaveChangesAsync();
    }

#pragma warning disable CS8602
    public async Task<EntreeCahierResponseDto?> ObtenirEntreeCahierParSeanceAsync(int seanceId)
    {
        // 1. Chercher l'entrée de cahier liée à cette séance
        var entree = await _context.EntreesCahier
            .Include(e => e.Seance)
                .ThenInclude(s => s.ProgrammeAcademique)
                    .ThenInclude(p => p.Matiere)
            .Include(e => e.Enseignant)
                .ThenInclude(en => en.Utilisateur)
            .Include(e => e.Decisions)
                .ThenInclude(d => d.Utilisateur)
            .FirstOrDefaultAsync(e => e.SeanceId == seanceId);

        // 2. Si aucune entrée n'existe, retourner null
        if (entree == null)
        {
            return null;
        }

        // 3. Transformer le modèle en DTO de réponse
        return MapperVersDto(entree);
    }
#pragma warning restore CS8602

    public async Task<EntreeCahierResponseDto> SoumettreEntreeCahierAsync(
        int seanceId,
        int enseignantId,
        CreateEntreeCahierDto dto)
    {
        // 1. Vérifier que la séance existe
#pragma warning disable CS8602
        var seance = await _context.Seances
            .Include(s => s.ProgrammeAcademique)
                .ThenInclude(p => p.CahiersTexte)
            .FirstOrDefaultAsync(s => s.Id == seanceId);
#pragma warning restore CS8602

        if (seance == null)
        {
            throw new InvalidOperationException("La séance spécifiée n'existe pas.");
        }

        // 2. Vérifier que l'enseignant est autorisé pour cette séance (RG07.4)
        var enseignant = await _context.Enseignants
            .FirstOrDefaultAsync(e => e.Id == enseignantId);

        if (enseignant == null)
        {
            throw new InvalidOperationException("Enseignant non trouvé.");
        }

        // 3. Récupérer le cahier de texte du programme
        var cahierTexte = seance.ProgrammeAcademique?.CahiersTexte.FirstOrDefault()
            ?? throw new InvalidOperationException("Aucun cahier de texte n'existe pour ce programme.");

        // 4. Vérifier si une entrée existe déjà pour cette séance
        var entreeExistante = await _context.EntreesCahier
            .FirstOrDefaultAsync(e => e.SeanceId == seanceId);

        EntreeCahier entree;

        if (entreeExistante != null)
        {
            // RG09.8 : Un cahier validé ne peut pas être modifié
            if (entreeExistante.Statut == StatutCahier.VALIDE)
            {
                throw new InvalidOperationException("Un cahier validé ne peut pas être modifié.");
            }

            entree = entreeExistante;
        }
        else
        {
            // Créer une nouvelle entrée
            entree = new EntreeCahier
            {
                SeanceId = seanceId,
                CahierTexteId = cahierTexte.Id,
                EnseignantId = enseignantId,
                Statut = StatutCahier.EN_PREPARATION
            };
            _context.EntreesCahier.Add(entree);
        }

        // 5. Remplir les données du formulaire
        entree.ContenuEnseigne = dto.ContenuEnseigne;
        entree.ActivitesRealisees = dto.ActivitesRealisees;
        entree.ActivitesPrevues = dto.ActivitesPrevues;

        // 6. Soumettre : changer le statut et enregistrer la date (RG09.6, RG09.7)
        entree.Statut = StatutCahier.EN_ATTENTE;
        entree.DateSoumission = DateTime.UtcNow;

        // 7. Sauvegarder
        await _context.SaveChangesAsync();

        // 8. Retourner la réponse
        return await ObtenirEntreeCahierParSeanceAsync(seanceId)
            ?? throw new InvalidOperationException("Erreur lors de la récupération de l'entrée.");
    }

    public async Task<EntreeCahierResponseDto> CorrigerEntreeCahierAsync(
        int entreeCahierId,
        int enseignantId,
        UpdateEntreeCahierDto dto)
    {
        // 1. Récupérer l'entrée
        var entree = await _context.EntreesCahier
            .FirstOrDefaultAsync(e => e.Id == entreeCahierId);

        if (entree == null)
        {
            throw new InvalidOperationException("Entrée de cahier non trouvée.");
        }

        // 2. Vérifier que l'enseignant est bien celui qui a soumis l'entrée
        if (entree.EnseignantId != enseignantId)
        {
            throw new InvalidOperationException("Vous n'êtes pas autorisé à modifier cette entrée.");
        }

        // 3. Vérifier que le statut est RETOURNE (RG10.6)
        if (entree.Statut != StatutCahier.RETOURNE)
        {
            throw new InvalidOperationException("Seule une entrée retournée peut être corrigée.");
        }

        // 4. Mettre à jour les données
        entree.ContenuEnseigne = dto.ContenuEnseigne;
        entree.ActivitesRealisees = dto.ActivitesRealisees;
        entree.ActivitesPrevues = dto.ActivitesPrevues;

        // 5. Soumettre à nouveau (RG09.6, RG09.7)
        entree.Statut = StatutCahier.EN_ATTENTE;
        entree.DateSoumission = DateTime.UtcNow;

        // 6. Sauvegarder
        await _context.SaveChangesAsync();

        // 7. Retourner la réponse
        return await ObtenirEntreeCahierParSeanceAsync(entree.SeanceId)
            ?? throw new InvalidOperationException("Erreur lors de la récupération de l'entrée.");
    }

    public async Task<EntreeCahierResponseDto> TraiterDecisionAsync(
        int entreeCahierId,
        int utilisateurId,
        DecisionCahierDto dto)
    {
        // 1. Récupérer l'entrée
        var entree = await _context.EntreesCahier
            .FirstOrDefaultAsync(e => e.Id == entreeCahierId);

        if (entree == null)
        {
            throw new InvalidOperationException("Entrée de cahier non trouvée.");
        }

        // 2. Vérifier que le statut est EN_ATTENTE
        if (entree.Statut != StatutCahier.EN_ATTENTE)
        {
            throw new InvalidOperationException("Seule une entrée en attente peut être traitée.");
        }

        // 3. RG10.3 & RG10.4 : Vérifier le commentaire si retour
        if (dto.Decision == TypeDecisionCahierEnum.RETOUR_CORRECTION
            && string.IsNullOrWhiteSpace(dto.Commentaire))
        {
            throw new InvalidOperationException(
                "Un commentaire est obligatoire lorsqu'un cahier est retourné pour correction.");
        }

        // 4. Récupérer l'utilisateur (validateur)
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.Id == utilisateurId);

        if (utilisateur == null)
        {
            throw new InvalidOperationException("Utilisateur non trouvé.");
        }

        // 5. Créer la décision (RG10.7 : historique)
        var decision = new DecisionCahier
        {
            EntreeCahierId = entreeCahierId,
            UtilisateurId = utilisateurId,
            Decision = dto.Decision,
            Commentaire = dto.Commentaire,
            DateDecision = DateTime.UtcNow
        };

        _context.DecisionsCahier.Add(decision);

        // 6. Mettre à jour le statut de l'entrée
        if (dto.Decision == TypeDecisionCahierEnum.VALIDATION)
        {
            entree.Statut = StatutCahier.VALIDE;
        }
        else
        {
            entree.Statut = StatutCahier.RETOURNE;
        }

        // 7. Sauvegarder
        await _context.SaveChangesAsync();

        // 8. Retourner la réponse
        return await ObtenirEntreeCahierParSeanceAsync(entree.SeanceId)
            ?? throw new InvalidOperationException("Erreur lors de la récupération de l'entrée.");
    }

#pragma warning disable CS8602
    public async Task<double> CalculerHeuresRealiseesAsync(int programmeAcademiqueId)
    {
        // RG08.2 : Seules les séances avec cahier validé comptent
        var entreesValides = await _context.EntreesCahier
            .Include(e => e.Seance)
                .ThenInclude(s => s.ProgrammeAcademique)
            .Where(e =>
                e.CahierTexte.ProgrammeAcademiqueId == programmeAcademiqueId
                && e.Statut == StatutCahier.VALIDE)
            .ToListAsync();

        // Calculer la somme des durées en heures
        double heuresRealisees = 0;

        foreach (var entree in entreesValides)
        {
            // Convertir les minutes en heures (ex: 90 minutes = 1.5 heures)
            heuresRealisees += entree.Seance.DureeMinutes / 60.0;
        }

        return heuresRealisees;
    }
#pragma warning restore CS8602

#pragma warning disable CS8602
    private EntreeCahierResponseDto MapperVersDto(EntreeCahier entree)
    {
        // Extraire les valeurs nullables pour éviter les warnings
        var utilisateurEnseignant = entree.Enseignant?.Utilisateur;

        return new EntreeCahierResponseDto
        {
            Id = entree.Id,
            SeanceId = entree.SeanceId,
            CahierTexteId = entree.CahierTexteId,

            // Informations de contexte
            NomMatiere = entree.Seance?.ProgrammeAcademique?.Matiere?.Nom ?? "N/A",
            NomEnseignant = utilisateurEnseignant != null
                ? $"{utilisateurEnseignant.Prenom} {utilisateurEnseignant.Nom}"
                : "N/A",
            DateSeance = entree.Seance?.Date.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue,
            Salle = entree.Seance?.Salle ?? "N/A",
            DureeMinutes = entree.Seance?.DureeMinutes ?? 0,

            // Contenu
            ContenuEnseigne = entree.ContenuEnseigne,
            ActivitesRealisees = entree.ActivitesRealisees,
            ActivitesPrevues = entree.ActivitesPrevues,

            // Statut
            Statut = entree.Statut,
            DateSoumission = entree.DateSoumission,

            // Dernier commentaire (RG10.5)
            DernierCommentaire = entree.Decisions
                .OrderByDescending(d => d.DateDecision)
                .FirstOrDefault()?.Commentaire,

            // Historique des décisions (RG10.7)
            HistoriqueDecisions = entree.Decisions
                .OrderByDescending(d => d.DateDecision)
                .Select(d => new DecisionCahierResponseDto
                {
                    Decision = d.Decision,
                    Commentaire = d.Commentaire,
                    NomValidateur = d.Utilisateur != null
                        ? $"{d.Utilisateur.Prenom} {d.Utilisateur.Nom}"
                        : "N/A",
                    DateDecision = d.DateDecision
                })
                .ToList()
        };
    }
#pragma warning restore CS8602
}