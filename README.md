# TradeMarket — Layered Marketplace Web API

A multi-layered **ASP.NET Core 8 Web API** application for managing a trading marketplace (customers, products, categories, receipts, and analytics), built with **Layered Architecture**, **Entity Framework Core**, **AutoMapper**, and full **unit & integration test suites** across every layer.

## Architecture

- **`Data`** — EF Core `DbContext`, domain entities (`Customer`, `Product`, `ProductCategory`, `Receipt`, `ReceiptDetail`), and Repository / Unit of Work patterns (SQLite).
- **`Business`** — Domain services, DTOs, AutoMapper profiles (`AutomapperProfile.cs`), and custom business validation (`MarketException`).
- **`WebApi`** — ASP.NET Core 8 RESTful controllers, dependency injection setup, and Swagger/OpenAPI documentation.
- **Test Suites:**
  - `Data.Tests` — Data access and repository tests
  - `Business.Tests` — Business service, statistics, and validation tests
  - `WebApi.Tests` — Controller & API integration tests
- **Code Quality:** Enforced with `stylecop.json` and `.editorconfig`.

## Tech Stack

- **Language & Runtime:** C# 12 / .NET 8
- **Web & API:** ASP.NET Core Web API, Swashbuckle (Swagger/OpenAPI)
- **Data & Mapping:** Entity Framework Core 8, SQLite, AutoMapper
- **Testing & Quality:** Automated tests (`Data.Tests`, `Business.Tests`, `WebApi.Tests`), StyleCop

## Getting Started

```bash
# Run all unit & integration tests
dotnet test

# Run the Web API
cd WebApi
dotnet run
```
