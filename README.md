# E-Commerce Product Catalog API

An ASP.NET Core learning project that exposes a product catalog through a REST-style HTTP API. It demonstrates separating controllers, application services, domain entities, and database access across multiple projects.

The implemented scope is catalog browsing. This repository is a reference implementation in progress, rather than a complete online store.

## Live API

[Open Swagger UI](https://mahmoud-ecommerce-api.runasp.net/swagger/index.html)

Base URL: https://mahmoud-ecommerce-api.runasp.net

## Features

- Browse products with their brand and type names.
- Retrieve a product by ID.
- Filter products by brand and type, and search by product name.
- Sort by name or price in ascending or descending order.
- Request paginated results with a default page size of 6 and a maximum of 10.
- Retrieve the available brands and product types.
- Map entities to DTOs with AutoMapper.
- Build product image URLs from a configurable base URL.
- Initialize a SQL Server database from bundled JSON seed data.
- Explore the API using Swagger UI in Development.

## Technology

- .NET 10 / ASP.NET Core Web API; the Domain project currently targets .NET 9.
- Entity Framework Core 9.0.17 with the SQL Server provider.
- AutoMapper 15.1.3.
- ASP.NET Core OpenAPI and Swagger UI.
- Dependency injection, generic Repository, Unit of Work, and Specification patterns.

Package versions and target frameworks are declared in the individual `.csproj` files. The solution currently mixes framework and package major versions; it has not been standardized onto a single version.

## Solution structure

```text
E.Commerce/
├── E-Commerce.Domain/                 # Entities and persistence contracts
├── E-Commerce.Shared/                 # DTOs, query parameters, pagination result
├── E-Commerce.Service.Abstraction/    # Application service contracts
├── E-Commerce.Service/               # Services, mapping, specifications
├── E-Commerce.Persistence/           # EF Core, repositories, database seeding
├── E-Commerce.Presentation.API/      # Product API controllers
├── E.Commerce.Web/                    # Startup, configuration, static images
├── E.Commerce.slnx
└── E.Commerce.sln
```

The web host registers dependencies and configures the HTTP pipeline. Controllers call `IProductService`; the product service builds specifications, queries repositories through `IUnitOfWork`, and maps entities into response DTOs.

```text
HTTP request → ProductController → IProductService
             → IUnitOfWork → Repository → StoreDbContext → SQL Server
             ← JSON DTOs ← AutoMapper ← queried entities
```

These are logical responsibilities. The current project references are not a strict Clean Architecture dependency graph; for example, Persistence references Service.

## Data model

- `Product`: integer ID, name, description, decimal price, image path, brand ID, and type ID.
- `ProductBrand`: integer ID and name.
- `ProductType`: integer ID and name.

Each product belongs to one brand and one type. The API response exposes the related names through `ProductDto`. Its `Price` property is currently a string, although the domain price is decimal.

## Run locally

### Prerequisites

- .NET 10 SDK, with .NET 9 targeting support for the Domain project.
- SQL Server or SQL Server Express LocalDB (the default Windows configuration).
- Git and a terminal, or Visual Studio with compatible .NET tooling.

### 1. Clone and build

```powershell
git clone https://github.com/MahmoudHosni2000/E.Commerce.git
cd E.Commerce
dotnet restore E.Commerce.slnx
dotnet build E.Commerce.slnx
```

### 2. Configure a disposable database

The initializer uses `EnsureCreated()` and inserts seed records only when their tables are empty. It no longer deletes the database at startup. Use a dedicated database for this application; `EnsureCreated()` is not a migration strategy for upgrading existing schemas.

Override the connection string in the current PowerShell session:

```powershell
$env:ConnectionStrings__DefaultConnection = 'Server=(localdb)\mssqllocaldb;Database=ECommerceCatalogDemo;Trusted_Connection=True;TrustServerCertificate=True'
$env:baseUrl = 'https://localhost:7139'
```

The checked-in `appsettings.json` contains a local integrated-authentication connection string, with no database password. For a remote database, supply your own connection string through environment variables or a local secret store. Never commit real credentials.

`baseUrl` controls the prefix added to product image paths. Set it to the externally reachable application URL when changing host or port.

### 3. Start the API

```powershell
dotnet dev-certs https --trust
dotnet run --project E.Commerce.Web --launch-profile https
```

- Swagger UI: https://localhost:7139/swagger
- OpenAPI document: https://localhost:7139/openapi/v1.json
- HTTP launch address: http://localhost:5045

Swagger UI and the OpenAPI document are enabled in Development, or when configuration `EnableSwagger` is true. For hosting, set `EnableSwagger=true`, configure `baseUrl`, and supply `ConnectionStrings__DefaultConnection` through private deployment settings.

JSON seed files are copied to `Data/DataSeed` in both build and publish output. The initializer reads them relative to the application directory.

## Product endpoints

All product actions use GET. The route template is `api/[controller]/[action]`. ASP.NET Core's default action naming removes the `Async` suffix from these method names.

- `GET /api/Product/GetProducts`: paginated catalog; accepts the parameters below.
- `GET /api/Product/GetById?id=1`: product lookup by ID.
- `GET /api/Product/GetBrands`: all brands.
- `GET /api/Product/GetTypes`: all product types.

### Catalog query parameters

- `TypeId`: optional integer product type filter.
- `BrandId`: optional integer brand filter.
- `Search`: optional product-name substring search.
- `Sort`: `NameAsc`, `NameDesc`, `PriceAsc`, or `PriceDesc`; defaults to name ascending.
- `PageIndex`: page number, starting at 1; default 1.
- `PageSize`: default 6, capped at 10. Use a positive value; lower-bound validation is not implemented.

Example requests from PowerShell:

```powershell
curl.exe 'https://localhost:7139/api/Product/GetProducts?PageIndex=1&PageSize=6&Sort=PriceAsc'
curl.exe 'https://localhost:7139/api/Product/GetProducts?BrandId=1&TypeId=2&Search=shoe'
curl.exe 'https://localhost:7139/api/Product/GetById?id=1'
curl.exe 'https://localhost:7139/api/Product/GetBrands'
curl.exe 'https://localhost:7139/api/Product/GetTypes'
```

Illustrative response shape (the values below are examples, not a captured API response):

```json
{
  "pageIndex": 1,
  "pageSize": 6,
  "totalCount": 1,
  "data": [
    {
      "id": 1,
      "name": "Example product",
      "description": "Example description",
      "pictureUrl": "https://localhost:7139/Images/products/example.png",
      "price": "100.00",
      "type": "Example type",
      "brand": "Example brand"
    }
  ]
}
```

The generated WeatherForecast controller is also present as template code; it is not part of the catalog domain.

## Current limitations

- No user accounts, JWT authentication, cart, checkout, orders, or payment integration is implemented.
- Product endpoints expose read operations; repository write methods are not exposed as product CRUD endpoints.
- An unknown product ID currently returns HTTP 200 with an empty `ProductDto`, rather than HTTP 404.
- `totalCount` currently uses the paginated product specification, so it can count only the requested page rather than every matching product. Treat this pagination metadata as a known issue.
- Page index and page size need lower-bound validation.
- Schema creation uses `EnsureCreated()`; versioned schema migrations are not yet implemented.
- Seed data is added only when each catalog table is empty.
- There is no automated test project or CI workflow in this snapshot.
- The demo is hosted on MonsterASP; availability depends on the free hosting service.

## Verification

Restore and build the solution using the commands above. To check runtime behavior, start it against a disposable SQL Server database, open Swagger, and exercise the product, brand, and type endpoints. Check filtering, all four sort options, pagination, missing IDs, and image URLs.

Compilation alone does not verify database connectivity, initialization, or endpoint responses.

## Suggested next steps

1. Add a versioned migration strategy for schema upgrades.
2. Correct the total-count query and add pagination validation.
3. Return a proper 404 response for missing products and standardize error responses.
4. Align framework/package versions.
5. Add integration tests and CI before extending the API with authentication, carts, or orders.

## Author

[Mahmoud Hosni](https://github.com/MahmoudHosni2000)
