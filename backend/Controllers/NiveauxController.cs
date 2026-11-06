
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NiveauxController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NiveauxController(ApplicationDbContext context)
    {
        _context = context;
    }

    
[HttpPost]
public async Task<IActionResult> Create(
    [FromBody] CreerNiveauDto dto)
{
    // 1. Vérifier le nom
    if (string.IsNullOrWhiteSpace(dto.Nom))
    {
        return BadRequest(new
        {
            message = "Le nom du niveau est obligatoire."
        });
    }

    // 2. Nettoyer le nom
    var nom = dto.Nom.Trim();

    // 3. Vérifier si ce niveau existe déjà
    var existe = await _context.Niveaux
        .AnyAsync(n => n.Nom.ToLower() == nom.ToLower());

    if (existe)
    {
        return Conflict(new
        {
            message = "Un niveau portant ce nom existe déjà."
        });
    }

    // 4. Créer le niveau
    var niveau = new Niveau
    {
        Nom = nom,
        Description = string.IsNullOrWhiteSpace(dto.Description)
            ? null
            : dto.Description.Trim()
    };

    // 5. Enregistrer dans SQLite
    _context.Niveaux.Add(niveau);
    await _context.SaveChangesAsync();

    // 6. Retourner le résultat
    return StatusCode(StatusCodes.Status201Created, niveau);
}


[HttpGet]
public async Task<IActionResult> GetAll()
{
    var niveaux = await _context.Niveaux
        .AsNoTracking()
        .OrderBy(n => n.Nom)
        .ToListAsync();

    return Ok(niveaux);
}


[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)
{
    var niveau = await _context.Niveaux
        .AsNoTracking()
        .Include(n => n.Classes)
        .FirstOrDefaultAsync(n => n.Id == id);

    if (niveau == null)
    {
        return NotFound(new
        {
            message = "Niveau introuvable."
        });
    }

    return Ok(niveau);
}


}
