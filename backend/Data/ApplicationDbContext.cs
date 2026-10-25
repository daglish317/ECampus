using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<AnneeAcademique> AnneesAcademiques
        => Set<AnneeAcademique>();

    public DbSet<Semestre> Semestres
        => Set<Semestre>();

    public DbSet<Niveau> Niveaux
        => Set<Niveau>();

    public DbSet<Classe> Classes
        => Set<Classe>();

    public DbSet<Matiere> Matieres
        => Set<Matiere>();

    public DbSet<InscriptionAcademique> InscriptionsAcademiques
        => Set<InscriptionAcademique>();

    public DbSet<ProgrammeAcademique> ProgrammesAcademiques
        => Set<ProgrammeAcademique>();

    public DbSet<Utilisateur> Utilisateurs
        => Set<Utilisateur>();

    public DbSet<Etudiant> Etudiants 
        => Set<Etudiant>();

    public DbSet<Enseignant> Enseignants 
        => Set<Enseignant>();

    public DbSet<AffectationEnseignant> AffectationsEnseignants
        => Set<AffectationEnseignant>();

    public DbSet<Seance> Seances 
        => Set<Seance>();

    public DbSet<ProgrammeHebdomadaire> ProgrammesHebdomadaires
        => Set<ProgrammeHebdomadaire>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Une seule classe par étudiant et par année académique.
        modelBuilder.Entity<InscriptionAcademique>()
            .HasIndex(i => new
            {
                i.EtudiantId,
                i.AnneeAcademiqueId
            })
            .IsUnique();

        
        // Le nom d'une année académique doit être unique.
        modelBuilder.Entity<AnneeAcademique>()
            .HasIndex(a => a.Nom)
            .IsUnique();

        // Une seule année académique peut être active.
        modelBuilder.Entity<AnneeAcademique>()
            .HasIndex(a => a.EstActive)
            .IsUnique()
            .HasFilter("\"EstActive\" = 1");
        
        
        modelBuilder.Entity<AnneeAcademique>()
            .ToTable(table =>
                table.HasCheckConstraint(
                    "CK_AnneeAcademique_Dates",
                    "\"DateFin\" >= \"DateDebut\""
                ));


        // Une classe appartient à un niveau.
        modelBuilder.Entity<Classe>()
            .HasOne(c => c.Niveau)
            .WithMany(n => n.Classes)
            .HasForeignKey(c => c.NiveauId)
            .OnDelete(DeleteBehavior.Restrict);

        // Un semestre appartient à une année académique.
        modelBuilder.Entity<Semestre>()
            .HasOne(s => s.AnneeAcademique)
            .WithMany()
            .HasForeignKey(s => s.AnneeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);

        // Une inscription concerne une classe et une année.
        modelBuilder.Entity<InscriptionAcademique>()
            .HasOne(i => i.Classe)
            .WithMany(c => c.Inscriptions)
            .HasForeignKey(i => i.ClasseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InscriptionAcademique>()
            .HasOne(i => i.AnneeAcademique)
            .WithMany()
            .HasForeignKey(i => i.AnneeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<InscriptionAcademique>()
            .HasOne(i => i.Etudiant)
            .WithMany()
            .HasForeignKey(i => i.EtudiantId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<InscriptionAcademique>()
            .HasOne(i => i.Etudiant)
            .WithMany()
            .HasForeignKey(i => i.EtudiantId)
            .OnDelete(DeleteBehavior.Restrict);


        // Une association de programme référence ses quatre éléments.
        modelBuilder.Entity<ProgrammeAcademique>()
            .HasOne(p => p.Matiere)
            .WithMany(m => m.Programmes)
            .HasForeignKey(p => p.MatiereId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProgrammeAcademique>()
            .HasOne(p => p.Niveau)
            .WithMany()
            .HasForeignKey(p => p.NiveauId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProgrammeAcademique>()
            .HasOne(p => p.AnneeAcademique)
            .WithMany()
            .HasForeignKey(p => p.AnneeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProgrammeAcademique>()
            .HasOne(p => p.Semestre)
            .WithMany()
            .HasForeignKey(p => p.SemestreId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<ProgrammeAcademique>()
            .HasIndex(p => new
            {
                p.MatiereId,
                p.NiveauId,
                p.AnneeAcademiqueId,
                p.SemestreId
            })
            .IsUnique();

        
        // Un compte utilisateur ne peut être lié
        // qu'à un seul profil étudiant.
        modelBuilder.Entity<Etudiant>()
            .HasOne(e => e.Utilisateur)
            .WithOne()
            .HasForeignKey<Etudiant>(e => e.UtilisateurId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Etudiant>()
            .HasIndex(e => e.UtilisateurId)
            .IsUnique();

        // Un compte utilisateur ne peut être lié
        // qu'à un seul profil enseignant.
        modelBuilder.Entity<Enseignant>()
            .HasOne(e => e.Utilisateur)
            .WithOne()
            .HasForeignKey<Enseignant>(e => e.UtilisateurId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Enseignant>()
            .HasIndex(e => e.UtilisateurId)
            .IsUnique();

        // Chaque compte Clerk doit être unique.
        modelBuilder.Entity<Utilisateur>()
            .HasIndex(u => u.ClerkUserId)
            .IsUnique();

        
        // Une affectation concerne un enseignant.
        modelBuilder.Entity<AffectationEnseignant>()
            .HasOne(a => a.Enseignant)
            .WithMany()
            .HasForeignKey(a => a.EnseignantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Une affectation concerne un programme académique.
        modelBuilder.Entity<AffectationEnseignant>()
            .HasOne(a => a.ProgrammeAcademique)
            .WithMany()
            .HasForeignKey(a => a.ProgrammeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<AffectationEnseignant>()
            .ToTable(table =>
                table.HasCheckConstraint(
                    "CK_AffectationEnseignant_Dates",
                    "\"DateFin\" IS NULL OR \"DateFin\" >= \"DateDebut\""
                ));


        
        modelBuilder.Entity<Seance>()
            .HasOne(s => s.ProgrammeAcademique)
            .WithMany()
            .HasForeignKey(s => s.ProgrammeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Seance>()
            .HasOne(s => s.AffectationEnseignant)
            .WithMany()
            .HasForeignKey(s => s.AffectationEnseignantId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<Seance>()
            .HasOne(s => s.ProgrammeHebdomadaire)
            .WithMany()
            .HasForeignKey(s => s.ProgrammeHebdomadaireId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<Seance>()
            .ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Seance_DureePositive",
                    "\"DureeMinutes\" > 0"
                );
            });

        
        modelBuilder.Entity<Semestre>()
            .ToTable(table =>
                table.HasCheckConstraint(
                    "CK_Semestre_Dates",
                    "\"DateFin\" >= \"DateDebut\""
                ));

        
        modelBuilder.Entity<ProgrammeHebdomadaire>()
            .HasOne(p => p.AnneeAcademique)
            .WithMany()
            .HasForeignKey(p => p.AnneeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}