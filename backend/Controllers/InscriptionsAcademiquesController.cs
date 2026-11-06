
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscriptionsAcademiquesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InscriptionsAcademiquesController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreerInscriptionAcademiqueDto dto)
    {
        // 1. Vérifier l'existence de l'étudiant
        var etudiantExiste = await _context.Etudiants
            .AnyAsync(e => e.Id == dto.EtudiantId);

        if (!etudiantExiste)
        {
            return NotFound(new
            {
                message = "Étudiant introuvable."
            });
        }

        // 2. Vérifier l'existence de la classe
        var classeExiste = await _context.Classes
            .AnyAsync(c => c.Id == dto.ClasseId);

        if (!classeExiste)
        {
            return NotFound(new
            {
                message = "Classe introuvable."
            });
        }

        // 3. Vérifier l'existence de l'année académique
        var annee = await _context.AnneesAcademiques
            .FirstOrDefaultAsync(a => a.Id == dto.AnneeAcademiqueId);

        if (annee == null)
        {
            return NotFound(new
            {
                message = "Année académique introuvable."
            });
        }

        // 4. Vérifier que l'année est active
        if (!annee.EstActive)
        {
            return BadRequest(new
            {
                message = "Impossible d'inscrire un étudiant dans une année inactive."
            });
        }

        // 5. Vérifier les doublons
        var inscriptionExiste = await _context.InscriptionsAcademiques
            .AnyAsync(i =>
                i.EtudiantId == dto.EtudiantId &&
                i.AnneeAcademiqueId == dto.AnneeAcademiqueId);

        if (inscriptionExiste)
        {
            return Conflict(new
            {
                message = "Cet étudiant est déjà inscrit pour cette année académique."
            });
        }

        // 6. Créer l'inscription
        var inscription = new InscriptionAcademique
        {
            EtudiantId = dto.EtudiantId,
            ClasseId = dto.ClasseId,
            AnneeAcademiqueId = dto.AnneeAcademiqueId
        };

        _context.InscriptionsAcademiques.Add(inscription);

        await _context.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Inscription académique créée avec succès.",
            inscriptionId = inscription.Id,
            etudiantId = inscription.EtudiantId,
            classeId = inscription.ClasseId,
            anneeAcademiqueId = inscription.AnneeAcademiqueId
        });
    }
}
