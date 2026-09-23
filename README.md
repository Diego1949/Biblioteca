# Biblioteca â€” API de consulta de catÃ¡logo

Seguimiento 1 Â· ProgramaciÃ³n Distribuida

API REST de **solo lectura** sobre un catÃ¡logo de libros, construida con Clean Architecture,
DDD y CQRS. Expone tres consultas: listado de libros, detalle por Id y filtrado por categorÃ­a.

## Stack

| Componente | VersiÃ³n |
|---|---|
| .NET | 8.0 |
| Entity Framework Core | 8.0.31 (SQL Server) |
| MediatR | 12.4.1 |
| Swashbuckle (Swagger) | 6.6.2 |
| Base de datos | SQL Server LocalDB |

## Arquitectura

```
src/
â”œâ”€â”€ Biblioteca.Domain           Entidades y reglas de negocio. Sin dependencias.
â”œâ”€â”€ Biblioteca.Application      Queries, Handlers, DTOs e interfaces de repositorio.
â”œâ”€â”€ Biblioteca.Infrastructure   DbContext, Fluent API, repositorios, migraciones y seed.
â””â”€â”€ Biblioteca.Api              Controllers, DI y Swagger.
```

La regla de dependencia apunta siempre hacia adentro:

```
Api â”€â”€> Infrastructure â”€â”€> Application â”€â”€> Domain
 â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”´â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
```

`Application` define *quÃ©* necesita (`IBookRepository`); `Infrastructure` decide *cÃ³mo* lo resuelve
(EF Core). El dominio no conoce a ninguno de los dos.

## Requisitos previos

1. **SDK de .NET 8.** El repositorio incluye un `global.json` que fija la versiÃ³n, asÃ­ que si
   tienes varios SDK instalados se usarÃ¡ el 8.0 automÃ¡ticamente.

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
   # si no estÃ¡ instalada:
   dotnet tool install --global dotnet-ef
   ```

## 1. Configurar la base de datos

La cadena de conexiÃ³n vive en `src/Biblioteca.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "BibliotecaDb": "Server=(localdb)\\MSSQLLocalDB;Database=BibliotecaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

Si usas otra instancia de SQL Server, este es el Ãºnico valor que debes cambiar.
No hace falta crear la base de datos a mano: las migraciones la generan.

## 2. Aplicar migraciones

Desde la raÃ­z del repositorio:

```bash
dotnet restore
dotnet ef database update -p src/Biblioteca.Infrastructure -s src/Biblioteca.Api
```

Esto crea la base `BibliotecaDb`, sus tres tablas y **carga los datos semilla**. El proyecto
tiene dos migraciones:

| MigraciÃ³n | Contenido |
|---|---|
| `InitialCreate` | Tablas `Books`, `Authors`, `Categories`, Ã­ndices y llaves forÃ¡neas |
| `SeedData` | 3 autores, 3 categorÃ­as y 5 libros |

**Para empezar de cero** (borra todos los datos):

```bash
dotnet ef database drop -p src/Biblioteca.Infrastructure -s src/Biblioteca.Api --force
dotnet ef database update -p src/Biblioteca.Infrastructure -s src/Biblioteca.Api
```

**Para generar el script SQL** en lugar de aplicar la migraciÃ³n (Ãºtil si prefieres ejecutarlo
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

> Los puertos estÃ¡n fijados en `Properties/launchSettings.json` y tienen prioridad sobre la
> variable `ASPNETCORE_URLS`. Para usar otro puerto:
> `dotnet run --project src/Biblioteca.Api --urls http://localhost:5080`

## 4. Probar los endpoints

### Query 1 â€” Listar todos los libros

```bash
curl http://localhost:5282/api/books
```

`200 OK` con los 5 libros ordenados por tÃ­tulo:

```json
[
  {
    "id": 1,
    "title": "Cien aÃ±os de soledad",
    "isbn": "9780307474728",
    "publicationYear": 1967,
    "author": "Gabriel GarcÃ­a MÃ¡rquez",
    "category": "Novela"
  }
]
```

### Query 2 â€” Detalle de un libro por Id

```bash
curl http://localhost:5282/api/books/3
```

`200 OK` con el autor y la categorÃ­a completos:

```json
{
  "id": 3,
  "title": "FundaciÃ³n",
  "isbn": "9788497596695",
  "publicationYear": 1951,
  "author": {
    "id": 2,
    "fullName": "Isaac Asimov",
    "biography": "Escritor y bioquÃ­mico estadounidense, autor del ciclo de la FundaciÃ³n."
  },
  "category": {
    "id": 2,
    "name": "Ciencia FicciÃ³n",
    "description": "Relatos construidos sobre avances cientÃ­ficos reales o imaginados."
  }
}
```

`404 Not Found` si el Id no existe: `curl -i http://localhost:5282/api/books/999`

### Query 3 â€” Libros por categorÃ­a

```bash
curl http://localhost:5282/api/books/category/2
```

`200 OK` con los libros de esa categorÃ­a, o `404 Not Found` si la categorÃ­a no existe.
Una categorÃ­a que existe pero no tiene libros devuelve `200` con una lista vacÃ­a.

### Resumen

| Verbo | Ruta | Respuestas |
|---|---|---|
| GET | `/api/books` | `200` |
| GET | `/api/books/{id}` | `200` Â· `404` |
| GET | `/api/books/category/{categoryId}` | `200` Â· `404` |

## Datos semilla

| Id | TÃ­tulo | AÃ±o | Autor | CategorÃ­a |
|---|---|---|---|---|
| 1 | Cien aÃ±os de soledad | 1967 | Gabriel GarcÃ­a MÃ¡rquez | Novela (1) |
| 2 | El amor en los tiempos del cÃ³lera | 1985 | Gabriel GarcÃ­a MÃ¡rquez | Novela (1) |
| 3 | FundaciÃ³n | 1951 | Isaac Asimov | Ciencia FicciÃ³n (2) |
| 4 | Yo, Robot | 1950 | Isaac Asimov | Ciencia FicciÃ³n (2) |
| 5 | Sapiens: De animales a dioses | 2011 | Yuval Noah Harari | Historia (3) |

Ids Ãºtiles para probar: categorÃ­a `1` y `2` tienen dos libros cada una, la `3` tiene uno,
y la `99` no existe (devuelve 404).

## Decisiones de diseÃ±o

- **Solo lectura.** No hay Create, Update ni Delete en esta versiÃ³n. Las consultas usan
  `AsNoTracking()` porque el change tracker de EF Core solo agrega costo cuando nadie escribe.
- **Mapeo manual en lugar de AutoMapper.** Con cuatro DTOs, unos mÃ©todos de extensiÃ³n son mÃ¡s
  rÃ¡pidos, no usan reflexiÃ³n y fallan en tiempo de compilaciÃ³n en vez de en runtime.
- **EncapsulaciÃ³n en el dominio.** Las entidades tienen setters privados y validan sus
  invariantes en el constructor: no se puede construir un `Book` sin tÃ­tulo o con un ISBN
  que no tenga 10 o 13 caracteres.
- **404 vs lista vacÃ­a.** El handler de la Query 3 comprueba primero si la categorÃ­a existe,
  para distinguir "la categorÃ­a no existe" de "la categorÃ­a no tiene libros".
- **`CancellationToken` en toda la cadena**, desde el controller hasta EF Core: si el cliente
  corta la conexiÃ³n, la consulta se cancela en SQL Server.
