using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id).ValueGeneratedOnAdd();

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(b => b.Isbn)
            .IsRequired()
            .HasMaxLength(13)
            .IsUnicode(false);

        builder.Property(b => b.PublicationYear)
            .IsRequired();

        builder.HasIndex(b => b.Isbn).IsUnique();

        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
