# 🚀 ASISYA ProviderOptimizerService  
Microservicio de optimización de proveedores para la plataforma de asistencia vehicular **ASISYA**.

Este servicio calcula el **mejor proveedor disponible** para atender una solicitud de asistencia en función de:

- Distancia  
- ETA (tiempo estimado de llegada)  
- Carga de trabajo  
- Disponibilidad  
- Tipo de servicio requerido  
- Score/historial  

Forma parte de una arquitectura basada en microservicios que incluye:
- LocationService  
- AssistanceService  
- NotificationService  
- ProviderOptimizerService (este repositorio)

---

## 📌 1. Características principales

- API REST construida con **.NET 8**
- Patrón **CQRS + MediatR**
- Arquitectura **DDD por capas**
- Mapper con **AutoMapper**
- Persistencia con **Entity Framework Core**
- Repositorios + Unit of Work
- JWT Authentication
- Dockerfile listo para despliegue
- Pruebas unitarias con **xUnit + Moq**  
- CI/CD con GitHub Actions (build, test, lint, docker)

---

## 📌 2. Arquitectura

Este microservicio sigue el modelo arquitectónico definido en ASISYA:

/ProviderOptimizer.API
/ProviderOptimizer.Application
/ProviderOptimizer.Domain
/ProviderOptimizer.Infrastructure


### 🏗 Patrones utilizados
- CQRS
- Mediator Pattern (MediatR)
- Repository Pattern
- Unit of Work
- SOLID
- Inyección de dependencias

---

## 📌 3. Diagramas (C4)

### Nivel 1 — System Context  
Describe la interacción entre el usuario, servicios externos y microservicios.

### Nivel 2 — Container Diagram  
Muestra los componentes:

- API Gateway  
- ProviderOptimizerService  
- Base de datos  
- LocationService  
- Notification Service

### Nivel 3 — Component Diagram  
Incluye:

- Controllers  
- Handlers  
- Servicios  
- Repositorios  
- DBContext  

*(Los diagramas están en la carpeta /docs del repositorio.)*

---

## 📌 4. Modelo de Datos (ERD)

Entidad principales:

- Provider  
- ProviderLocation  
- ProviderAvailability  
- ProviderWorkload  
- OptimizationRequest  
- OptimizationResult  

---

## 📌 5. Endpoints principales

### 🔹 **POST /api/optimizer/optimize**
Calcula el mejor proveedor.

### 🔹 **GET /api/optimizer/results/{requestId}**
Obtiene resultados de optimización.

### 🔹 **GET /api/providers/available**
Lista proveedores disponibles.

### 🔹 **PUT /api/providers/{id}/location**
Actualiza ubicación.

### 🔹 **PUT /api/providers/{id}/availability**
Actualiza disponibilidad.

### 🔹 **GET /api/health**
Health check del microservicio.

---

## 📌 6. Configuración del entorno

### Variables requeridas

```json
"ConnectionString": "Server=...;Database=ASISYA;Trusted_Connection=True;",
"Jwt": {
  "Issuer": "ASISYA",
  "Audience": "ASISYA_API",
  "Key": "clave-secreta-super-segura"
}
