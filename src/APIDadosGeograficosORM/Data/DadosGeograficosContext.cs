using Microsoft.EntityFrameworkCore;

namespace APIDadosGeograficosORM.Data;

public class DadosGeograficosContext : DbContext
{
    public DbSet<Regiao> Regioes { get; set; } = null!;
    public DbSet<Estado> Estados { get; set; } = null!;

    public DadosGeograficosContext(DbContextOptions<DadosGeograficosContext> options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Regiao>().HasMany(r => r.Estados);
        modelBuilder.Entity<Estado>().HasOne(e => e.Regiao);
    }
}