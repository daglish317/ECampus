
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClassesController(ApplicationDbContext context)
    {
        _context = context;
    }

    
[HttpPost]
public async Task<IActionResult> Create(
    [FromBody] CreerClasseDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Nom))
    {
        return BadRequest(new
        {
            message = "Le nom de la classe est obligatoire."
        });
    }

    var nom = dto.Nom.Trim();

    var niveau = await _context.Niveaux
        .FirstOrDefaultAsync(n => n.Id == dto.NiveauId);

    if (niveau == null)
    {
        return BadRequest(new
        {
            message = "Le niveau indiqué n'existe pas."
        });
    }

    var existe = await _context.Classes
        .AnyAsync(c =>
            c.NiveauId == dto.NiveauId &&
            c.Nom.ToLower() == nom.ToLower());

    if (existe)
    {
        return Conflict(new
        {
            message = "Une classe portant ce nom existe déjà dans ce niveau."
        });
    }

    var classe = new Classe
    {
        Nom = nom,
        NiveauId = dto.NiveauId
    };

    _context.Classes.Add(classe);
    await _context.SaveChangesAsync();

    return StatusCode(StatusCodes.Status201Created, classe);
}


[HttpGet]
public async Task<IActionResult> GetAll()
{
    var classes = await _context.Classes
        .AsNoTracking()
        .Include(c => c.Niveau)
        .OrderBy(c => c.Nom)
        .ToListAsync();

    return Ok(classes);
}

[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)
{
    var classe = await _context.Classes
        .AsNoTracking()
        .Include(c => c.Niveau)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (classe == null)
    {
        return NotFound(new
        {
            message = "Classe introuvable."
        });
    }

    return Ok(classe);
}



}
