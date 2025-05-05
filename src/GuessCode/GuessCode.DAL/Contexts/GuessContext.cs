using Microsoft.EntityFrameworkCore;

namespace GuessCode.DAL.Contexts;

public class GuessContext : DbContext
{
    public GuessContext(DbContextOptions<GuessContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GuessContext).Assembly);
    }
}