using Backend.Data;

namespace Backend.Services;

public class ProgrammeHebdomadaireService
{
    private readonly ApplicationDbContext _context;

    public ProgrammeHebdomadaireService(ApplicationDbContext context)
    {
        _context = context;
    }
}