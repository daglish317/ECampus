
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnseignantsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EnseignantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreerEnseignantDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nom) ||
            string.IsNullOrWhiteSpace(dto.Prenom) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest(new
            {
                message = "Le nom, le prénom et l'email sont obligatoires."
            });
        }

        var nom = dto.Nom.Trim();
        var prenom = dto.Prenom.Trim();
        var email = dto.Email.Trim().ToLowerInvariant();

        var emailExiste = await _context.Utilisateurs
            .AnyAsync(u => u.Email.ToLower() == email);

        if (emailExiste)
        {
            return Conflict(new
            {
                message = "Un utilisateur possède déjà cet email."
            });
        }

        var utilisateur = new Utilisateur
        {
            Nom = nom,
            Prenom = prenom,
            Email = email,
            Role = "Enseignant",
            ClerkUserId = $"pending_{Guid.NewGuid():N}"
        };

        var enseignant = new Enseignant
        {
            Utilisateur = utilisateur
        };

        _context.Enseignants.Add(enseignant);

        await _context.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Enseignant créé avec succès.",
            enseignantId = enseignant.Id,
            utilisateurId = utilisateur.Id,
            nom = utilisateur.Nom,
            prenom = utilisateur.Prenom,
            email = utilisateur.Email,
            role = utilisateur.Role,
            statutCompte = "En attente d'association Clerk"
        });
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var enseignants = await _context.Enseignants
            .AsNoTracking()
            .Include(e => e.Utilisateur)
            .OrderBy(e => e.Id)
            .Select(e => new
            {
                e.Id,
                e.UtilisateurId,
                Nom = e.Utilisateur!.Nom,
                Prenom = e.Utilisateur.Prenom,
                Email = e.Utilisateur.Email,
                Role = e.Utilisateur.Role
            })
            .ToListAsync();

        return Ok(enseignants);
    }

}
