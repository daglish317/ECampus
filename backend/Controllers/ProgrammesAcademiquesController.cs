
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgrammesAcademiquesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProgrammesAcademiquesController(ApplicationDbContext context)
    {
        _context = context;
    }

    
[HttpPost]
public async Task<IActionResult> Create(
    CreateProgrammeAcademiqueDto dto)
{
    // 1. Vérifier que la matière existe
    var matiere = await _context.Matieres
        .FindAsync(dto.MatiereId);

    if (matiere == null)
    {
        return NotFound("Matière introuvable.");
    }

    // 2. Vérifier que le niveau existe
    var niveau = await _context.Niveaux
        .FindAsync(dto.NiveauId);

    if (niveau == null)
    {
        return NotFound("Niveau introuvable.");
    }

    // 3. Vérifier que l'année académique existe
    var annee = await _context.AnneesAcademiques
        .FindAsync(dto.AnneeAcademiqueId);

    if (annee == null)
    {
        return NotFound("Année académique introuvable.");
    }

    // 4. Vérifier que le semestre existe
    var semestre = await _context.Semestres
        .FindAsync(dto.SemestreId);

    if (semestre == null)
    {
        return NotFound("Semestre introuvable.");
    }

    
    if (semestre.AnneeAcademiqueId != dto.AnneeAcademiqueId)
    {
        return BadRequest(
            "Le semestre sélectionné n'appartient pas à cette année académique.");
    }


    // 5. Vérifier la cohérence des dates
    if (dto.DateFinEffet.HasValue &&
        dto.DateFinEffet.Value < dto.DateEffet)
    {
        return BadRequest(
            "La date de fin d'effet doit être postérieure à la date de début.");
    }

    

if (dto.DateEffet.Date < annee.DateDebut.ToDateTime(TimeOnly.MinValue).Date ||
    dto.DateEffet.Date > annee.DateFin.ToDateTime(TimeOnly.MinValue).Date)
{
    return BadRequest(
        "La date d'effet doit être comprise dans l'année académique.");
}

if (dto.DateFinEffet.HasValue &&
    dto.DateFinEffet.Value.Date > annee.DateFin.ToDateTime(TimeOnly.MinValue).Date)
{
    return BadRequest(
        "La date de fin d'effet ne peut pas dépasser la fin de l'année académique.");
}


    // 6. Vérifier les doublons
    var existe = await _context.ProgrammesAcademiques
        .AnyAsync(p =>
            p.MatiereId == dto.MatiereId &&
            p.NiveauId == dto.NiveauId &&
            p.AnneeAcademiqueId == dto.AnneeAcademiqueId &&
            p.SemestreId == dto.SemestreId);

    if (existe)
    {
        return BadRequest(
            "Cette matière est déjà associée à ce niveau, cette année et ce semestre.");
    }

    // 7. Créer le programme
    var programme = new ProgrammeAcademique
    {
        MatiereId = dto.MatiereId,
        NiveauId = dto.NiveauId,
        AnneeAcademiqueId = dto.AnneeAcademiqueId,
        SemestreId = dto.SemestreId,
        DateEffet = dto.DateEffet,
        DateFinEffet = dto.DateFinEffet
    };

    
_context.ProgrammesAcademiques.Add(programme);

await _context.SaveChangesAsync();

var response = new ProgrammeAcademiqueResponseDto
{
    Id = programme.Id,

    MatiereId = programme.MatiereId,
    NomMatiere = matiere.Nom,

    NiveauId = programme.NiveauId,
    NomNiveau = niveau.Nom,

    AnneeAcademiqueId = programme.AnneeAcademiqueId,
    NomAnneeAcademique = annee.Nom,

    SemestreId = programme.SemestreId,
    NomSemestre = semestre.Nom,

    DateEffet = programme.DateEffet,
    DateFinEffet = programme.DateFinEffet
};

return CreatedAtAction(
    nameof(GetById),
    new { id = programme.Id },
    response
);

}

[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var programme = await _context.ProgrammesAcademiques
        .Include(p => p.Matiere)
        .Include(p => p.Niveau)
        .Include(p => p.AnneeAcademique)
        .Include(p => p.Semestre)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (programme == null)
    {
        return NotFound("Programme académique introuvable.");
    }

    var response = new ProgrammeAcademiqueResponseDto
    {
        Id = programme.Id,

        MatiereId = programme.MatiereId,
        NomMatiere = programme.Matiere!.Nom,

        NiveauId = programme.NiveauId,
        NomNiveau = programme.Niveau!.Nom,

        AnneeAcademiqueId = programme.AnneeAcademiqueId,
        NomAnneeAcademique = programme.AnneeAcademique!.Nom,

        SemestreId = programme.SemestreId,
        NomSemestre = programme.Semestre!.Nom,

        DateEffet = programme.DateEffet,
        DateFinEffet = programme.DateFinEffet
    };

    return Ok(response);
}


[HttpGet]
public async Task<IActionResult> GetAll()
{
    var programmes = await _context.ProgrammesAcademiques
        .Include(p => p.Matiere)
        .Include(p => p.Niveau)
        .Include(p => p.AnneeAcademique)
        .Include(p => p.Semestre)
        .ToListAsync();

    var response = programmes.Select(programme =>
        new ProgrammeAcademiqueResponseDto
        {
            Id = programme.Id,

            MatiereId = programme.MatiereId,
            NomMatiere = programme.Matiere!.Nom,

            NiveauId = programme.NiveauId,
            NomNiveau = programme.Niveau!.Nom,

            AnneeAcademiqueId = programme.AnneeAcademiqueId,
            NomAnneeAcademique = programme.AnneeAcademique!.Nom,

            SemestreId = programme.SemestreId,
            NomSemestre = programme.Semestre!.Nom,

            DateEffet = programme.DateEffet,
            DateFinEffet = programme.DateFinEffet
        }).ToList();

    return Ok(response);
}


[HttpPost("{id}/changer")]
public async Task<IActionResult> ChangerProgramme(
    int id,
    ChangerProgrammeAcademiqueDto dto)
{
    // 1. Retrouver l'ancien programme
    var ancienProgramme = await _context.ProgrammesAcademiques
        .FirstOrDefaultAsync(p => p.Id == id);

    if (ancienProgramme == null)
    {
        return NotFound("Programme académique introuvable.");
    }

    // 2. Vérifier que l'ancienne version est encore active
    if (ancienProgramme.DateFinEffet.HasValue &&
    ancienProgramme.DateFinEffet.Value.Date < dto.DateEffet.Date)
{
    return BadRequest(
        "Ce programme est déjà terminé à la date du changement demandé.");
}

    // 3. Vérifier la date du changement
    if (dto.DateEffet.Date <= ancienProgramme.DateEffet.Date)
    {
        return BadRequest(
            "La nouvelle date d'effet doit être postérieure à celle de l'ancien programme.");
    }

    // 4. Vérifier les nouvelles entités
    var matiere = await _context.Matieres.FindAsync(dto.MatiereId);
    var niveau = await _context.Niveaux.FindAsync(dto.NiveauId);
    var annee = await _context.AnneesAcademiques.FindAsync(dto.AnneeAcademiqueId);
    var semestre = await _context.Semestres.FindAsync(dto.SemestreId);

    if (matiere == null)
        return NotFound("Matière introuvable.");

    if (niveau == null)
        return NotFound("Niveau introuvable.");

    if (annee == null)
        return NotFound("Année académique introuvable.");

    if (semestre == null)
        return NotFound("Semestre introuvable.");

    if (semestre.AnneeAcademiqueId != dto.AnneeAcademiqueId)
        return BadRequest("Le semestre n'appartient pas à cette année académique.");

    if (dto.DateEffet.Date < annee.DateDebut.ToDateTime(TimeOnly.MinValue).Date ||
        dto.DateEffet.Date > annee.DateFin.ToDateTime(TimeOnly.MinValue).Date)
    {
        return BadRequest(
            "La date d'effet doit être comprise dans l'année académique.");
    }

    // 5. Vérifier que le nouveau contexte n'existe pas déjà
    var existe = await _context.ProgrammesAcademiques
        .AnyAsync(p =>
            p.MatiereId == dto.MatiereId &&
            p.NiveauId == dto.NiveauId &&
            p.AnneeAcademiqueId == dto.AnneeAcademiqueId &&
            p.SemestreId == dto.SemestreId &&
            p.DateEffet == dto.DateEffet);

    if (existe)
    {
        return BadRequest(
            "Cette association existe déjà à cette date.");
    }

    // 6. Clôturer l'ancienne version et créer la nouvelle
    await using var transaction =
        await _context.Database.BeginTransactionAsync();

    try
    {
        ancienProgramme.DateFinEffet = dto.DateEffet.Date.AddDays(-1);

        var nouveauProgramme = new ProgrammeAcademique
        {
            MatiereId = dto.MatiereId,
            NiveauId = dto.NiveauId,
            AnneeAcademiqueId = dto.AnneeAcademiqueId,
            SemestreId = dto.SemestreId,
            DateEffet = dto.DateEffet,
            DateFinEffet = null
        };

        _context.ProgrammesAcademiques.Add(nouveauProgramme);

        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = nouveauProgramme.Id },
            new
            {
                ancienProgrammeId = ancienProgramme.Id,
                nouveauProgrammeId = nouveauProgramme.Id,
                dateFinAncienProgramme = ancienProgramme.DateFinEffet,
                dateEffetNouveauProgramme = nouveauProgramme.DateEffet
            });
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}




}
