using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Infrastructure.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Biography)
            .HasMaxLength(1000);

        builder.Ignore(a => a.FullName);

        builder.Metadata
            .FindNavigation(nameof(Author.Books))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(a => new { a.LastName, a.FirstName });
    }
}
