# Watchwi Backend

## Descripción General

Watchwi es una aplicación backend diseñada para gestionar una plataforma de contenido multimedia, enfocada en la autenticación de usuarios y el acceso estructurado a un catálogo de medios como películas y series.

El proyecto se construye desde cero con un fuerte enfoque en arquitectura limpia, mantenibilidad y separación de responsabilidades. Sirve como una implementación práctica de buenas prácticas modernas de desarrollo backend utilizando ASP.NET Core y tecnologías relacionadas.

Este repositorio contiene la API backend responsable de la autenticación, la gestión de contenido multimedia y la integración con servicios externos.

---

## Arquitectura

La solución sigue los principios de **Clean Architecture** combinados con conceptos de **Domain-Driven Design (DDD)**.

El proyecto se estructura en cuatro capas principales:

- Watchwi.Domain
- Watchwi.Application
- Watchwi.Infrastructure
- Watchwi.Api

### Responsabilidad de las Capas

- **Domain**
  - Reglas de negocio centrales
  - Entidades, value objects y contratos del dominio
  - No depende de frameworks externos

- **Application**
  - Casos de uso y lógica de aplicación
  - DTOs, interfaces y validaciones
  - Depende únicamente de la capa Domain

- **Infrastructure**
  - Acceso a base de datos (Entity Framework Core)
  - Servicios externos (JWT, Cloudinary, etc.)
  - Implementa interfaces definidas en Domain y Application

- **API**
  - Endpoints HTTP y controladores
  - Middleware de autenticación y autorización
  - Configuración e inyección de dependencias

---

## Stack Tecnológico

### Backend

- C# / ASP.NET Core 8
- Entity Framework Core
- Autenticación JWT
- MySQL
- Cloudinary
- Swagger / OpenAPI

### Frontend

- Vue.js (repositorio separado)

---

## Funcionalidades

- Registro y autenticación de usuarios
- Autorización basada en JWT
- Gestión de catálogo multimedia (películas y series)
- Manejo de contenido favorito
- Subida segura de archivos y almacenamiento de medios
- Diseño de API REST
- Documentación automática con Swagger

---

## Estructura del Proyecto

docs/

- diagrams (clases, entidad-relación, secuencia, casos de uso) [diagramas](./docs/diagrams/README.md)

src/

- Watchwi.Api
- Watchwi.Application
- Watchwi.Domain
- Watchwi.Infrastructure

---

## Notas de Desarrollo

- La API es completamente stateless.
- Las reglas de negocio están aisladas de la infraestructura.
- El proyecto prioriza claridad, escalabilidad y mantenibilidad.
- Se incluyen archivos `.http` para pruebas manuales de endpoints.

---

## Inicio Rápido

### Requisitos Previos

- .NET 8 SDK
- MySQL
- Cuenta de Cloudinary (opcional)

### Ejecutar la API

dotnet restore  
dotnet build  
dotnet run --project src/Watchwi.Api

La interfaz de Swagger estará disponible en:

`https://localhost:{puerto}/swagger`

---

## Estado

Este proyecto se encuentra en desarrollo activo y está orientado a fines educativos y de práctica arquitectónica.

---

## Licencia

Proyecto destinado a aprendizaje y demostración.

---

### english version / versión en ingles

[Ir a la versión en ingles](./README.md)
