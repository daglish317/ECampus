using Backend.Models;
using Backend.DTOs;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnneesAcademiquesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AnneesAcademiquesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var annees = await _context.AnneesAcademiques
            .AsNoTracking()
            .OrderByDescending(a => a.DateDebut)
            .ToListAsync();

        return Ok(annees);
    }

    
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreerAnneeAcademiqueDto dto)
    {
        // 1. Vérifier le nom
        if (string.IsNullOrWhiteSpace(dto.Nom))
        {
            return BadRequest(new
            {
                message = "Le nom de l'année académique est obligatoire."
            });
        }

        // 2. Vérifier les dates
        if (dto.DateDebut == default || dto.DateFin == default)
        {
            return BadRequest(new
            {
                message = "Les dates de début et de fin sont obligatoires."
            });
        }

        if (dto.DateFin < dto.DateDebut)
        {
            return BadRequest(new
            {
                message = "La date de fin doit être postérieure ou égale à la date de début."
            });
        }

        // 3. Nettoyer le nom
        var nom = dto.Nom.Trim();

        // 4. Vérifier si le nom existe déjà
        var existe = await _context.AnneesAcademiques
            .AnyAsync(a => a.Nom == nom);

        if (existe)
        {
            return Conflict(new
            {
                message = "Cette année académique existe déjà."
            });
        }

        // 5. Préparer l'année académique
        var annee = new AnneeAcademique
        {
            Nom = nom,
            DateDebut = dto.DateDebut,
            DateFin = dto.DateFin,
            EstActive = false
        };

        // 6. Enregistrer dans SQLite
        _context.AnneesAcademiques.Add(annee);
        await _context.SaveChangesAsync();

        // 7. Retourner la réponse
        return StatusCode(StatusCodes.Status201Created, annee);
    }

    
    [HttpPut("{id:int}/activation")]
    public async Task<IActionResult> Activer(int id)
    {
        // 1. Rechercher l'année demandée
        var annee = await _context.AnneesAcademiques
            .FirstOrDefaultAsync(a => a.Id == id);

        if (annee == null)
        {
            return NotFound(new
            {
                message = "Année académique introuvable."
            });
        }

        // 2. Vérifier si elle est déjà active
        if (annee.EstActive)
        {
            return Ok(annee);
        }

        // 3. Commencer une transaction
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        // 4. Désactiver l'année actuellement active
        var anneeActive = await _context.AnneesAcademiques
            .FirstOrDefaultAsync(a => a.EstActive);

        if (anneeActive != null)
        {
            anneeActive.EstActive = false;
            await _context.SaveChangesAsync();
        }

        // 5. Activer la nouvelle année
        annee.EstActive = true;
        await _context.SaveChangesAsync();

        // 6. Valider la transaction
        await transaction.CommitAsync();

        return Ok(annee);
    }

    
    [HttpPut("{id:int}/desactivation")]
    public async Task<IActionResult> Desactiver(int id)
    {
        var annee = await _context.AnneesAcademiques
            .FirstOrDefaultAsync(a => a.Id == id);

        if (annee == null)
        {
            return NotFound(new
            {
                message = "Année académique introuvable."
            });
        }

        if (!annee.EstActive)
        {
            return Ok(annee);
        }

        annee.EstActive = false;

        await _context.SaveChangesAsync();

        return Ok(annee);
    }



}
