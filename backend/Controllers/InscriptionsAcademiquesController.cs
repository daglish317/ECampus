
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

    [HttpGet]
public async Task<IActionResult> GetAll()
{
    var inscriptions = await _context.InscriptionsAcademiques
        .AsNoTracking()
        .Include(i => i.Etudiant)
            .ThenInclude(e => e!.Utilisateur)
        .Include(i => i.Classe)
        .Include(i => i.AnneeAcademique)
        .Select(i => new InscriptionAcademiqueResponseDto
        {
            Id = i.Id,

            EtudiantId = i.EtudiantId,
            NomEtudiant = i.Etudiant!.Utilisateur!.Nom,
            PrenomEtudiant = i.Etudiant.Utilisateur.Prenom,
            EmailEtudiant = i.Etudiant.Utilisateur.Email,

            ClasseId = i.ClasseId,
            NomClasse = i.Classe!.Nom,

            AnneeAcademiqueId = i.AnneeAcademiqueId,
            NomAnneeAcademique = i.AnneeAcademique!.Nom
        })
        .ToListAsync();

    return Ok(inscriptions);
}

[HttpGet("etudiant/{etudiantId}")]
public async Task<IActionResult> GetByEtudiant(int etudiantId)
{
    var inscriptions = await _context.InscriptionsAcademiques
        .AsNoTracking()
        .Where(i => i.EtudiantId == etudiantId)
        .Include(i => i.Etudiant)
            .ThenInclude(e => e!.Utilisateur)
        .Include(i => i.Classe)
        .Include(i => i.AnneeAcademique)
        .Select(i => new InscriptionAcademiqueResponseDto
        {
            Id = i.Id,

            EtudiantId = i.EtudiantId,
            NomEtudiant = i.Etudiant!.Utilisateur!.Nom,
            PrenomEtudiant = i.Etudiant.Utilisateur.Prenom,
            EmailEtudiant = i.Etudiant.Utilisateur.Email,

            ClasseId = i.ClasseId,
            NomClasse = i.Classe!.Nom,

            AnneeAcademiqueId = i.AnneeAcademiqueId,
            NomAnneeAcademique = i.AnneeAcademique!.Nom
        })
        .ToListAsync();

    if (!inscriptions.Any())
    {
        return NotFound(
            $"Aucune inscription trouvée pour l'étudiant {etudiantId}."
        );
    }

    return Ok(inscriptions);
}

[HttpPut("{id}")]
public async Task<IActionResult> Modifier(
    int id,
    ModifierInscriptionAcademiqueDto dto)
{
    // 1. Vérifier si l'inscription existe
    var inscription = await _context.InscriptionsAcademiques
        .FirstOrDefaultAsync(i => i.Id == id);

    if (inscription == null)
    {
        return NotFound($"Inscription {id} introuvable.");
    }

    // 2. Vérifier si la classe existe
    var classeExiste = await _context.Classes
        .AnyAsync(c => c.Id == dto.ClasseId);

    if (!classeExiste)
    {
        return NotFound($"Classe {dto.ClasseId} introuvable.");
    }

    // 3. Vérifier si l'année académique existe
    var anneeAcademique = await _context.AnneesAcademiques
        .FirstOrDefaultAsync(a => a.Id == dto.AnneeAcademiqueId);

    if (anneeAcademique == null)
    {
        return NotFound(
            $"Année académique {dto.AnneeAcademiqueId} introuvable."
        );
    }

    // 4. Vérifier si l'année est active
    if (!anneeAcademique.EstActive)
    {
        return BadRequest(
            "Impossible de modifier l'inscription vers une année inactive."
        );
    }

    // 5. Vérifier les doublons
    var doublon = await _context.InscriptionsAcademiques
        .AnyAsync(i =>
            i.EtudiantId == inscription.EtudiantId &&
            i.AnneeAcademiqueId == dto.AnneeAcademiqueId &&
            i.Id != id);

    if (doublon)
    {
        return Conflict(
            "Cet étudiant possède déjà une inscription pour cette année académique."
        );
    }

    // 6. Modifier l'inscription
    inscription.ClasseId = dto.ClasseId;
    inscription.AnneeAcademiqueId = dto.AnneeAcademiqueId;

    // 7. Enregistrer les changements
    await _context.SaveChangesAsync();

    return Ok(new
    {
        message = "Inscription académique modifiée avec succès.",
        inscriptionId = inscription.Id,
        classeId = inscription.ClasseId,
        anneeAcademiqueId = inscription.AnneeAcademiqueId
    });
}

}
