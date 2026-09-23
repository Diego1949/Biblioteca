using Biblioteca.Application;
using Biblioteca.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BibliotecaDb")
    ?? throw new InvalidOperationException("Connection string 'BibliotecaDb' was not found.");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
