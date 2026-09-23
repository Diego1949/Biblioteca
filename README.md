# Biblioteca — API de consulta de catálogo

Seguimiento 1 · Programación Distribuida

API REST de **solo lectura** sobre un catálogo de libros, construida con Clean Architecture,
DDD y CQRS. Expone tres consultas: listado de libros, detalle por Id y filtrado por categoría.

## Stack

| Componente | Versión |
|---|---|
| .NET | 8.0 |
| Entity Framework Core | 8.0.31 (SQL Server) |
| MediatR | 12.4.1 |
| Swashbuckle (Swagger) | 6.6.2 |
| Base de datos | SQL Server LocalDB |

## Arquitectura

```
src/
  Biblioteca.Domain           Entidades y reglas de negocio. Sin dependencias.
  Biblioteca.Application      Queries, Handlers, DTOs e interfaces de repositorio.
  Biblioteca.Infrastructure   DbContext, Fluent API, repositorios, migraciones y seed.
  Biblioteca.Api              Controllers, DI y Swagger.
```

La regla de dependencia apunta siempre hacia adentro:

```
Api -> Infrastructure -> Application -> Domain
```

`Application` define *qué* necesita (`IBookRepository`); `Infrastructure` decide *cómo* lo resuelve
(EF Core). El dominio no conoce a ninguno de los dos.

## Requisitos previos

1. **SDK de .NET 8.** El repositorio incluye un `global.json` que fija la versión, así que si
   tienes varios SDK instalados se usará el 8.0 automáticamente.

   ```bash
   dotnet --version    # debe reportar 8.0.x
   ```

2. **SQL Server LocalDB**, incluido con Visual Studio o con SQL Server Express.

   ```bash
   sqllocaldb info     # debe listar MSSQLLocalDB
   ```

3. **Herramienta `dotnet-ef`**, necesaria solo para aplicar migraciones.

   ```bash
   dotnet ef --version
   # si no está instalada:
   dotnet tool install --global dotnet-ef
   ```

## 1. Configurar la base de datos

La cadena de conexión vive en `src/Biblioteca.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "BibliotecaDb": "Server=(localdb)\\MSSQLLocalDB;Database=BibliotecaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

Si usas otra instancia de SQL Server, este es el único valor que debes cambiar.
No hace falta crear la base de datos a mano: las migraciones la generan.

## 2. Aplicar migraciones

Desde la raíz del repositorio:

```bash
dotnet restore
dotnet ef database update -p src/Biblioteca.Infrastructure -s src/Biblioteca.Api
```

Esto crea la base `BibliotecaDb`, sus tres tablas y **carga los datos semilla**. El proyecto
tiene dos migraciones:

| Migración | Contenido |
|---|---|
| `InitialCreate` | Tablas `Books`, `Authors`, `Categories`, índices y llaves foráneas |
| `SeedData` | 3 autores, 3 categorías y 5 libros |

**Para empezar de cero** (borra todos los datos):

```bash
dotnet ef database drop -p src/Biblioteca.Infrastructure -s src/Biblioteca.Api --force
dotnet ef database update -p src/Biblioteca.Infrastructure -s src/Biblioteca.Api
```

**Para generar el script SQL** en lugar de aplicar la migración (útil si prefieres ejecutarlo
desde SSMS):

```bash
dotnet ef migrations script -p src/Biblioteca.Infrastructure -s src/Biblioteca.Api -o biblioteca.sql
```

## 3. Levantar el proyecto

```bash
dotnet run --project src/Biblioteca.Api
```

| Perfil | URL |
|---|---|
| `http` (por defecto) | http://localhost:5282 |
| `https` | https://localhost:7247 |

Swagger UI queda en **http://localhost:5282/swagger** (solo en entorno `Development`).

> Los puertos están fijados en `Properties/launchSettings.json` y tienen prioridad sobre la
> variable `ASPNETCORE_URLS`. Para usar otro puerto:
> `dotnet run --project src/Biblioteca.Api --urls http://localhost:5080`

## 4. Probar los endpoints

### Query 1 — Listar todos los libros

```bash
curl http://localhost:5282/api/books
```

`200 OK` con los 5 libros ordenados por título:

```json
[
  {
    "id": 1,
    "title": "Cien años de soledad",
    "isbn": "9780307474728",
    "publicationYear": 1967,
    "author": "Gabriel García Márquez",
    "category": "Novela"
  }
]
```

### Query 2 — Detalle de un libro por Id

```bash
curl http://localhost:5282/api/books/3
```

`200 OK` con el autor y la categoría completos:

```json
{
  "id": 3,
  "title": "Fundación",
  "isbn": "9788497596695",
  "publicationYear": 1951,
  "author": {
    "id": 2,
    "fullName": "Isaac Asimov",
    "biography": "Escritor y bioquímico estadounidense, autor del ciclo de la Fundación."
  },
  "category": {
    "id": 2,
    "name": "Ciencia Ficción",
    "description": "Relatos construidos sobre avances científicos reales o imaginados."
  }
}
```

`404 Not Found` si el Id no existe: `curl -i http://localhost:5282/api/books/999`

### Query 3 — Libros por categoría

```bash
curl http://localhost:5282/api/books/category/2
```

`200 OK` con los libros de esa categoría, o `404 Not Found` si la categoría no existe.
Una categoría que existe pero no tiene libros devuelve `200` con una lista vacía.

### Resumen

| Verbo | Ruta | Respuestas |
|---|---|---|
| GET | `/api/books` | `200` |
| GET | `/api/books/{id}` | `200` · `404` |
| GET | `/api/books/category/{categoryId}` | `200` · `404` |

## Datos semilla

| Id | Título | Año | Autor | Categoría |
|---|---|---|---|---|
| 1 | Cien años de soledad | 1967 | Gabriel García Márquez | Novela (1) |
| 2 | El amor en los tiempos del cólera | 1985 | Gabriel García Márquez | Novela (1) |
| 3 | Fundación | 1951 | Isaac Asimov | Ciencia Ficción (2) |
| 4 | Yo, Robot | 1950 | Isaac Asimov | Ciencia Ficción (2) |
| 5 | Sapiens: De animales a dioses | 2011 | Yuval Noah Harari | Historia (3) |

Ids útiles para probar: categoría `1` y `2` tienen dos libros cada una, la `3` tiene uno,
y la `99` no existe (devuelve 404).

## Decisiones de diseño

- **Solo lectura.** No hay Create, Update ni Delete en esta versión. Las consultas usan
  `AsNoTracking()` porque el change tracker de EF Core solo agrega costo cuando nadie escribe.
- **Mapeo manual en lugar de AutoMapper.** Con cuatro DTOs, unos métodos de extensión son más
  rápidos, no usan reflexión y fallan en tiempo de compilación en vez de en runtime.
- **Encapsulación en el dominio.** Las entidades tienen setters privados y validan sus
  invariantes en el constructor: no se puede construir un `Book` sin título o con un ISBN
  que no tenga 10 o 13 caracteres.
- **404 vs lista vacía.** El handler de la Query 3 comprueba primero si la categoría existe,
  para distinguir "la categoría no existe" de "la categoría no tiene libros".
- **`CancellationToken` en toda la cadena**, desde el controller hasta EF Core: si el cliente
  corta la conexión, la consulta se cancela en SQL Server.
