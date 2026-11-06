
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EtudiantsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EtudiantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreerEtudiantDto dto)
    {
        // 1. Vérifier les champs obligatoires
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

        // 2. Vérifier si l'email existe déjà
        var emailExiste = await _context.Utilisateurs
            .AnyAsync(u => u.Email.ToLower() == email);

        if (emailExiste)
        {
            return Conflict(new
            {
                message = "Un utilisateur possède déjà cet email."
            });
        }

        // 3. Créer l'utilisateur
        var utilisateur = new Utilisateur
        {
            Nom = nom,
            Prenom = prenom,
            Email = email,
            Role = "Etudiant",

            // Identifiant provisoire en attendant Clerk
            ClerkUserId = $"pending_{Guid.NewGuid():N}"
        };

        // 4. Créer le profil étudiant
        var etudiant = new Etudiant
        {
            Utilisateur = utilisateur
        };

        // 5. Enregistrer les deux objets
        _context.Etudiants.Add(etudiant);

        await _context.SaveChangesAsync();

        // 6. Retourner une réponse
        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Étudiant créé avec succès.",
            etudiantId = etudiant.Id,
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
    var etudiants = await _context.Etudiants
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

    return Ok(etudiants);
}

[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)
{
    var etudiant = await _context.Etudiants
        .AsNoTracking()
        .Include(e => e.Utilisateur)
        .Where(e => e.Id == id)
        .Select(e => new
        {
            e.Id,
            e.UtilisateurId,
            Nom = e.Utilisateur!.Nom,
            Prenom = e.Utilisateur.Prenom,
            Email = e.Utilisateur.Email,
            Role = e.Utilisateur.Role
        })
        .FirstOrDefaultAsync();

    if (etudiant == null)
    {
        return NotFound(new
        {
            message = "Étudiant introuvable."
        });
    }

    return Ok(etudiant);
}
}
