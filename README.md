# Watchwi Backend

## Overview

Watchwi is a backend application designed to manage a multimedia content platform, focused on user authentication and structured access to a catalog of media such as movies and series.

The project is built from scratch with a strong emphasis on clean architecture, maintainability, and separation of concerns. It serves as a practical implementation of modern backend development practices using ASP.NET Core and related technologies.

This repository contains the backend API responsible for authentication, media management, and integration with external services.

---

## Architecture

The solution follows **Clean Architecture** principles combined with **Domain-Driven Design (DDD)** concepts.

The project is structured into four main layers:

- Watchwi.Domain
- Watchwi.Application
- Watchwi.Infrastructure
- Watchwi.Api

### Layer Responsibilities

- **Domain**
  - Core business rules
  - Entities, value objects, and domain contracts
  - No dependencies on external frameworks

- **Application**
  - Use cases and application logic
  - DTOs, interfaces, and validations
  - Depends only on the Domain layer

- **Infrastructure**
  - Database access (Entity Framework Core)
  - External services (JWT, Cloudinary, etc.)
  - Implements interfaces defined in Domain and Application

- **API**
  - HTTP endpoints and controllers
  - Authentication and authorization middleware
  - Dependency injection and configuration

---

## Technology Stack

### Backend

- C# / ASP.NET Core 8
- Entity Framework Core
- JWT Authentication
- MySQL
- Cloudinary
- Swagger / OpenAPI

### Frontend

- Vue.js (separate repository)

---

## Features

- User registration and authentication
- JWT-based authorization
- Media catalog management (movies and series)
- Favorite content handling
- Secure file upload and media storage
- RESTful API design
- API documentation with Swagger

---

## Project Structure

docs/

- diagrams (class, ER, sequence, use case) [diagrams](./docs/diagrams/README.md)

src/

- Watchwi.Api
- Watchwi.Application
- Watchwi.Domain
- Watchwi.Infrastructure

---

## Development Notes

- The API is stateless by design.
- Business rules are isolated from infrastructure concerns.
- The project prioritizes clarity, scalability, and maintainability.
- HTTP request files (.http) are included for manual endpoint testing.

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- MySQL
- Cloudinary account (optional)

### Run the API

dotnet restore  
dotnet build  
dotnet run --project src/Watchwi.Api

Swagger UI will be available at:

`https://localhost:{port}/swagger`

---

## Status

This project is under active development and intended for architectural and backend practice purposes.

---

## License

This project is provided for learning and demonstration purposes.

---

### spanish version / versión en español

[Ir a la versión en español](./README_ES.md)
