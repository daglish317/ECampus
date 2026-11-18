using Backend.DTOs;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AffectationsEnseignantsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AffectationsEnseignantsController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAffectationEnseignantDto dto)
    {
        // 1. Vérifier la cohérence des dates
        if (dto.DateFin.HasValue && dto.DateFin.Value < dto.DateDebut)
        {
            return BadRequest(new
            {
                message = "La date de fin ne peut pas être antérieure à la date de début."
            });
        }

        // 2. Vérifier l'existence de l'enseignant
        var enseignantExiste = await _context.Enseignants
            .AnyAsync(e => e.Id == dto.EnseignantId);

        if (!enseignantExiste)
        {
            return NotFound(new
            {
                message = "L'enseignant indiqué n'existe pas."
            });
        }

        // 3. Vérifier l'existence du programme académique
        var programmeExiste = await _context.ProgrammesAcademiques
            .AnyAsync(p => p.Id == dto.ProgrammeAcademiqueId);

        if (!programmeExiste)
        {
            return NotFound(new
            {
                message = "Le programme académique indiqué n'existe pas."
            });
        }

        // 4. Déterminer la période de l'affectation
        var dateFinEffective = dto.DateFin ?? DateOnly.MaxValue;

        // 5. Vérifier les chevauchements avec les affectations existantes
        var chevauchement = await _context.AffectationsEnseignants
            .AnyAsync(a =>
                a.ProgrammeAcademiqueId == dto.ProgrammeAcademiqueId
                && a.DateDebut <= dateFinEffective
                && (a.DateFin == null || a.DateFin >= dto.DateDebut)
            );

        if (chevauchement)
        {
            return Conflict(new
            {
                message = "Ce programme académique possède déjà une affectation sur cette période."
            });
        }

        // 6. Créer l'affectation
        var affectation = new AffectationEnseignant
        {
            EnseignantId = dto.EnseignantId,
            ProgrammeAcademiqueId = dto.ProgrammeAcademiqueId,
            DateDebut = dto.DateDebut,
            DateFin = dto.DateFin
        };

        _context.AffectationsEnseignants.Add(affectation);

        await _context.SaveChangesAsync();

        // 7. Retourner le résultat
        return Ok(new
        {
            message = "Affectation créée avec succès.",
            affectation = new
            {
                affectation.Id,
                affectation.EnseignantId,
                affectation.ProgrammeAcademiqueId,
                affectation.DateDebut,
                affectation.DateFin
            }
        });
    }


    [HttpPut("{id:int}/cloturer")]
    public async Task<IActionResult> Cloturer(
        int id,
        CloturerAffectationEnseignantDto dto)
    {
        // 1. Rechercher l'affectation
        var affectation = await _context.AffectationsEnseignants
            .FirstOrDefaultAsync(a => a.Id == id);

        if (affectation == null)
        {
            return NotFound(new
            {
                message = "Affectation introuvable."
            });
        }

        // 2. Vérifier que la date de fin est valide
        if (dto.DateFin < affectation.DateDebut)
        {
            return BadRequest(new
            {
                message = "La date de fin ne peut pas être antérieure à la date de début."
            });
        }

       // 3. Vérifier que la nouvelle date de fin est différente
        if (affectation.DateFin == dto.DateFin)
        {
            return Conflict(new
            {
                message = "Cette affectation possède déjà cette date de fin."
            });
        }


        // 4. Enregistrer la date de fin
        affectation.DateFin = dto.DateFin;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Affectation clôturée avec succès.",
            affectation = new
            {
                affectation.Id,
                affectation.EnseignantId,
                affectation.ProgrammeAcademiqueId,
                affectation.DateDebut,
                affectation.DateFin
            }
        });
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var affectations = await _context.AffectationsEnseignants
            .AsNoTracking()
            .Include(a => a.Enseignant)
                .ThenInclude(e => e!.Utilisateur)
            .Include(a => a.ProgrammeAcademique)
                .ThenInclude(p => p!.Matiere)
            .Include(a => a.ProgrammeAcademique)
                .ThenInclude(p => p!.Niveau)
            .OrderBy(a => a.ProgrammeAcademiqueId)
            .ThenBy(a => a.DateDebut)
            .Select(a => new
            {
                a.Id,
                a.EnseignantId,
                NomEnseignant = a.Enseignant!.Utilisateur!.Nom,
                PrenomEnseignant = a.Enseignant.Utilisateur.Prenom,
                EmailEnseignant = a.Enseignant.Utilisateur.Email,

                a.ProgrammeAcademiqueId,
                NomMatiere = a.ProgrammeAcademique!.Matiere!.Nom,
                NomNiveau = a.ProgrammeAcademique.Niveau!.Nom,

                a.DateDebut,
                a.DateFin
            })
            .ToListAsync();

        return Ok(affectations);
    }


}
