# WatchWi – System Design Documentation

Este repositorio contiene los diagramas oficiales de diseño del sistema WatchWi.  
Cada diagrama representa una vista específica de la arquitectura y el dominio.

---

## Entity–Relationship Diagram

Define el modelo de datos y las relaciones entre entidades persistentes.

![Entity Relationship Diagram](/docs/diagrams/watchwi-er-diagram.png)

---

## Domain Class Diagram

Representa el modelo de dominio y sus relaciones, independiente de la persistencia.

![Domain Class Diagram](/docs/diagrams/watchwi-class-diagram-domain.png)

Boceto del Diagrama de Clases del Dominio:

![Domain Class Diagram Sketch](/docs/diagrams/watchwi-class-diagram-sketch.png)

Represneta el modelo de dominio y sus relaciones, Final.

![Domain Class Diagram Database](/docs/diagrams/watchwi-class-diagram-database.png)

---

## Use Case Diagram

Describe las funcionalidades del sistema desde la perspectiva del usuario y del administrador.

![Use Case Diagram](/docs/diagrams/watchwi-use-case-diagram.png)

---

## Sequence Diagrams

### Add Media to Favorites

Detalla el flujo completo para agregar contenido a favoritos.

![Add Favorite Sequence](/docs/diagrams/watchwi-sequence-add-favorite.png)

### Remove Media from Favorites

Muestra el proceso de eliminación de un favorito existente.

![Remove Favorite Sequence](/docs/diagrams/watchwi-sequence-remove-favorite.png)

### View Media Catalog

Describe la consulta y visualización del catálogo de contenido.

![View Media Catalog Sequence](/docs/diagrams/watchwi-sequence-view-media-catalog.png)

### User Login

Explica el flujo de autenticación y generación de JWT.

![User Login Sequence](/docs/diagrams/watchwi-sequence-user-login.png)

---

## Component Diagram

Define la arquitectura del sistema siguiendo Clean Architecture y separación de capas.

![Component Diagram](/docs/diagrams/watchwi-component-diagram-clean-architecture.png)

---

## Dependency Diagram

Muestra las dependencias entre proyectos y capas en la solución WatchWi.

![Dependency Diagram](/docs/diagrams/watchwi-dependency-diagram.png)
