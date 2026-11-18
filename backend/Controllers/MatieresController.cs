
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatieresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MatieresController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET : api/matieres
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var matieres = await _context.Matieres
            .OrderBy(m => m.Nom)
            .ToListAsync();

        return Ok(matieres);
    }


[HttpPost]
public async Task<IActionResult> Create(Matiere matiere)
{
    if (string.IsNullOrWhiteSpace(matiere.Nom))
    {
        return BadRequest("Le nom de la matière est obligatoire.");
    }

    var nom = matiere.Nom.Trim();

    var existe = await _context.Matieres
        .AnyAsync(m => m.Nom.ToLower() == nom.ToLower());

    if (existe)
    {
        return BadRequest("Cette matière existe déjà.");
    }

    var nouvelleMatiere = new Matiere
    {
        Nom = nom
    };

    _context.Matieres.Add(nouvelleMatiere);

    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetAll),
        new { id = nouvelleMatiere.Id },
        nouvelleMatiere
    );
}

}
