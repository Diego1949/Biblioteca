using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infrastructure.Persistence.Seed;

public static class ModelBuilderSeedExtensions
{
    public static void ApplySeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new
            {
                Id = 1,
                Name = "Novela",
                Description = (string?)"Obras narrativas de ficción en prosa."
            },
            new
            {
                Id = 2,
                Name = "Ciencia Ficción",
                Description = (string?)"Relatos construidos sobre avances científicos reales o imaginados."
            },
            new
            {
                Id = 3,
                Name = "Historia",
                Description = (string?)"Obras que estudian y narran hechos del pasado."
            });

        modelBuilder.Entity<Author>().HasData(
            new
            {
                Id = 1,
                FirstName = "Gabriel",
                LastName = "García Márquez",
                Biography = (string?)"Escritor y periodista colombiano, Premio Nobel de Literatura en 1982."
            },
            new
            {
                Id = 2,
                FirstName = "Isaac",
                LastName = "Asimov",
                Biography = (string?)"Escritor y bioquímico estadounidense, autor del ciclo de la Fundación."
            },
            new
            {
                Id = 3,
                FirstName = "Yuval Noah",
                LastName = "Harari",
                Biography = (string?)"Historiador israelí especializado en historia global y macrohistoria."
            });

        modelBuilder.Entity<Book>().HasData(
            new
            {
                Id = 1,
                Title = "Cien años de soledad",
                Isbn = "9780307474728",
                PublicationYear = 1967,
                AuthorId = 1,
                CategoryId = 1
            },
            new
            {
                Id = 2,
                Title = "El amor en los tiempos del cólera",
                Isbn = "9780307389732",
                PublicationYear = 1985,
                AuthorId = 1,
                CategoryId = 1
            },
            new
            {
                Id = 3,
                Title = "Fundación",
                Isbn = "9788497596695",
                PublicationYear = 1951,
                AuthorId = 2,
                CategoryId = 2
            },
            new
            {
                Id = 4,
                Title = "Yo, Robot",
                Isbn = "9788435021326",
                PublicationYear = 1950,
                AuthorId = 2,
                CategoryId = 2
            },
            new
            {
                Id = 5,
                Title = "Sapiens: De animales a dioses",
                Isbn = "9788499926223",
                PublicationYear = 2011,
                AuthorId = 3,
                CategoryId = 3
            });
    }
}
