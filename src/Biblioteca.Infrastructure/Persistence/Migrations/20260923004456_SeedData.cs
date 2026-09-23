using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Biblioteca.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Escritor y periodista colombiano, Premio Nobel de Literatura en 1982.", "Gabriel", "García Márquez" },
                    { 2, "Escritor y bioquímico estadounidense, autor del ciclo de la Fundación.", "Isaac", "Asimov" },
                    { 3, "Historiador israelí especializado en historia global y macrohistoria.", "Yuval Noah", "Harari" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Obras narrativas de ficción en prosa.", "Novela" },
                    { 2, "Relatos construidos sobre avances científicos reales o imaginados.", "Ciencia Ficción" },
                    { 3, "Obras que estudian y narran hechos del pasado.", "Historia" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "CategoryId", "Isbn", "PublicationYear", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1, "9780307474728", 1967, "Cien años de soledad" },
                    { 2, 1, 1, "9780307389732", 1985, "El amor en los tiempos del cólera" },
                    { 3, 2, 2, "9788497596695", 1951, "Fundación" },
                    { 4, 2, 2, "9788435021326", 1950, "Yo, Robot" },
                    { 5, 3, 3, "9788499926223", 2011, "Sapiens: De animales a dioses" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
