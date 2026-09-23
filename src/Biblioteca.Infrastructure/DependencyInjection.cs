using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Infrastructure.Persistence;
using Biblioteca.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<BibliotecaDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
                sql.MigrationsAssembly(typeof(BibliotecaDbContext).Assembly.FullName)));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
