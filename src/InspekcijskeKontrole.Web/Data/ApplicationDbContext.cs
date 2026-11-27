using InspekcijskeKontrole.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Proizvod> Proizvodi => Set<Proizvod>();

    public DbSet<InspekcijskoTijelo> InspekcijskaTijela => Set<InspekcijskoTijelo>();

    public DbSet<InspekcijskaKontrola> InspekcijskeKontrole => Set<InspekcijskaKontrola>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
