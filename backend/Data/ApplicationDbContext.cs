using Backend.Models;
using Backend.Models.ETextBook; 
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

    // ==========================================
    // e-TEXTBOOK (EF07, EF09, EF10)
    // ==========================================
    public DbSet<CahierTexte> CahiersTexte => Set<CahierTexte>();
    public DbSet<EntreeCahier> EntreesCahier => Set<EntreeCahier>();
    public DbSet<DecisionCahier> DecisionsCahier => Set<DecisionCahier>();

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
                p.SemestreId,
                p.DateEffet
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
            .HasOne(s => s.AffectationEnseignant)
            .WithMany()
            .HasForeignKey(s => s.AffectationEnseignantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Seance>()
            .HasOne(s => s.ProgrammeHebdomadaire)
            .WithMany(p => p.Seances)
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
 

        modelBuilder.Entity<ProgrammeHebdomadaire>()
            .HasOne(p => p.Classe)
            .WithMany(c => c.ProgrammesHebdomadaires)
            .HasForeignKey(p => p.ClasseId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Une version de programme peut succéder à une version précédente.
        modelBuilder.Entity<ProgrammeHebdomadaire>()
        .HasOne(p => p.ProgrammePrecedent)
        .WithOne(p => p.VersionSuivante)
        .HasForeignKey<ProgrammeHebdomadaire>(p => p.ProgrammePrecedentId)
        .OnDelete(DeleteBehavior.Restrict);
        
        // Un numéro de version doit être unique pour une classe, une année et une semaine données.
        modelBuilder.Entity<ProgrammeHebdomadaire>()
            .HasIndex(p => new
            {
                p.ClasseId,
                p.AnneeAcademiqueId,
                p.DateDebutSemaine,
                p.NumeroVersion
            })
            .IsUnique();

<<<<<<< HEAD
        // ==========================================
       // e-TEXTBOOK (EF07, EF09, EF10)
       // ==========================================

// RG07.2 : Un seul cahier de texte par programme académique
modelBuilder.Entity<CahierTexte>()
    .HasOne(c => c.ProgrammeAcademique)
    .WithMany(p => p.CahiersTexte)
    .HasForeignKey(c => c.ProgrammeAcademiqueId)
    .OnDelete(DeleteBehavior.Restrict); // RG07.6 (Pas de suppression en cascade)

modelBuilder.Entity<CahierTexte>()
    .HasIndex(c => c.ProgrammeAcademiqueId)
    .IsUnique();

// RG07.3 : Une seule entrée de cahier par séance
modelBuilder.Entity<EntreeCahier>()
    .HasOne(e => e.CahierTexte)
    .WithMany(c => c.Entrees)
    .HasForeignKey(e => e.CahierTexteId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<EntreeCahier>()
    .HasOne(e => e.Seance)
    .WithOne(s => s.EntreeCahier)
    .HasForeignKey<EntreeCahier>(e => e.SeanceId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<EntreeCahier>()
    .HasIndex(e => e.SeanceId)
    .IsUnique();

// RG07.5 : Le remplacement d'un enseignant ne supprime pas les entrées
modelBuilder.Entity<EntreeCahier>()
    .HasOne(e => e.Enseignant)
    .WithMany(en => en.EntreesCahier)
    .HasForeignKey(e => e.EnseignantId)
    .OnDelete(DeleteBehavior.Restrict);

// RG09.2 : Le contenu enseigné est obligatoire
modelBuilder.Entity<EntreeCahier>()
    .Property(e => e.ContenuEnseigne)
    .IsRequired()
    .HasMaxLength(2000);

modelBuilder.Entity<EntreeCahier>()
    .Property(e => e.ActivitesRealisees)
    .HasMaxLength(2000);

modelBuilder.Entity<EntreeCahier>()
    .Property(e => e.ActivitesPrevues)
    .HasMaxLength(2000);

// Historique des décisions (EF10)
modelBuilder.Entity<DecisionCahier>()
    .HasOne(d => d.EntreeCahier)
    .WithMany(e => e.Decisions)
    .HasForeignKey(d => d.EntreeCahierId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<DecisionCahier>()
    .HasOne(d => d.Utilisateur)
    .WithMany(u => u.DecisionsCahier)
    .HasForeignKey(d => d.UtilisateurId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<DecisionCahier>()
    .Property(d => d.Commentaire)
    .HasMaxLength(2000);

=======
        /* Une seule version publiée par classe,
         année académique et semaine.*/
        modelBuilder.Entity<ProgrammeHebdomadaire>()
            .HasIndex(p => new
            {
                p.ClasseId,
                p.AnneeAcademiqueId,
                p.DateDebutSemaine
            })
            .IsUnique()
            .HasFilter("\"Statut\" = 'Publie'");

        modelBuilder.Entity<ProgrammeHebdomadaire>()
        .HasIndex(p => p.ProgrammePrecedentId)
        .IsUnique()
        .HasFilter("\"ProgrammePrecedentId\" IS NOT NULL");
>>>>>>> 73966d6e7fcb6e41fe7e4a2c14da1677bc2c8aa6
    }
}