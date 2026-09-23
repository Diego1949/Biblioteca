using Biblioteca.Domain.Entities;
using Biblioteca.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infrastructure.Persistence;

public class BibliotecaDbContext : DbContext
{
    public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Author> Authors => Set<Author>();

    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BibliotecaDbContext).Assembly);
        modelBuilder.ApplySeedData();
        base.OnModelCreating(modelBuilder);
    }
}
