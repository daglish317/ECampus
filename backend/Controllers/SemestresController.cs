
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SemestresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SemestresController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var semestres = await _context.Semestres
            .AsNoTracking()
            .Include(s => s.AnneeAcademique)
            .OrderBy(s => s.DateDebut)
            .ToListAsync();

        return Ok(semestres);
    }

    
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreerSemestreDto dto)
    {
        // 1. Vérifier le nom
        if (string.IsNullOrWhiteSpace(dto.Nom))
        {
            return BadRequest(new
            {
                message = "Le nom du semestre est obligatoire."
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

        // 3. Rechercher l'année académique
        var annee = await _context.AnneesAcademiques
            .FirstOrDefaultAsync(a => a.Id == dto.AnneeAcademiqueId);

        if (annee == null)
        {
            return BadRequest(new
            {
                message = "L'année académique indiquée n'existe pas."
            });
        }

        // 4. Vérifier que le semestre reste dans l'année
        if (dto.DateDebut < annee.DateDebut ||
            dto.DateFin > annee.DateFin)
        {
            return BadRequest(new
            {
                message = "Les dates du semestre doivent être comprises dans celles de l'année académique."
            });
        }

        // 5. Créer le semestre
        var semestre = new Semestre
        {
            Nom = dto.Nom.Trim(),
            DateDebut = dto.DateDebut,
            DateFin = dto.DateFin,
            AnneeAcademiqueId = dto.AnneeAcademiqueId
        };

        // 6. Enregistrer dans la base
        _context.Semestres.Add(semestre);
        await _context.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, semestre);
    }

    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var semestre = await _context.Semestres
            .AsNoTracking()
            .Include(s => s.AnneeAcademique)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (semestre == null)
        {
            return NotFound(new
            {
                message = "Semestre introuvable."
            });
        }

        return Ok(semestre);
    }

    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] ModifierSemestreDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nom))
        {
            return BadRequest(new
            {
                message = "Le nom du semestre est obligatoire."
            });
        }

        if (dto.DateDebut == default || dto.DateFin == default)
        {
            return BadRequest(new
            {
                message = "Les dates sont obligatoires."
            });
        }

        if (dto.DateFin < dto.DateDebut)
        {
            return BadRequest(new
            {
                message = "La date de fin doit être postérieure ou égale à la date de début."
            });
        }

        var semestre = await _context.Semestres
            .Include(s => s.AnneeAcademique)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (semestre == null)
        {
            return NotFound(new
            {
                message = "Semestre introuvable."
            });
        }

        var annee = semestre.AnneeAcademique!;

        if (dto.DateDebut < annee.DateDebut ||
            dto.DateFin > annee.DateFin)
        {
            return BadRequest(new
            {
                message = "Les dates du semestre doivent rester dans celles de l'année académique."
            });
        }

        semestre.Nom = dto.Nom.Trim();
        semestre.DateDebut = dto.DateDebut;
        semestre.DateFin = dto.DateFin;

        await _context.SaveChangesAsync();

        return Ok(semestre);
    }



}




