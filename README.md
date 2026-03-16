# 🚗 GtMotive Estimate Microservice

Este proyecto es una API RESTful desarrollada en **.NET 9**, diseñada siguiendo estrictamente los principios de la **Arquitectura Hexagonal (Puertos y Adaptadores)** y el **Diseño Guiado por el Dominio (DDD)**.

El objetivo principal de este microservicio es gestionar el ciclo de vida del alquiler de vehículos, garantizando que las reglas de negocio se cumplan de forma aislada e independiente de la infraestructura tecnológica subyacente.

## 🏛️ Decisiones de Arquitectura

El proyecto está dividido en capas concéntricas para garantizar la inversión de dependencias (SOLID):

1. **Domain:** El corazón del software. Contiene los *Aggregates* (`Vehicle`), Entidades, *Value Objects* (`ManufactureDate`) y Excepciones de Dominio. No tiene dependencias externas.
2. **ApplicationCore:** Contiene los Casos de Uso (ej. `RentVehicleUseCase`). Orquesta la lógica de negocio utilizando interfaces (Puertos) para comunicarse con el exterior.
3. **Infrastructure:** Contiene los Adaptadores de salida. Implementa los repositorios utilizando **MongoDB** para la persistencia de datos.
4. **Api (Host):** El Adaptador de entrada. Expone los endpoints HTTP (Controladores) y actúa como punto de composición (Composition Root) inyectando las dependencias.

## 🛠️ Requisitos Previos

Para compilar, ejecutar y testear este proyecto, necesitarás:
* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Para levantar la infraestructura de base de datos)
* Un IDE compatible (Visual Studio 2022, JetBrains Rider o VS Code)

## 🚀 Cómo ejecutar la aplicación

1. **Levantar la base de datos:** El proyecto incluye un archivo `docker-compose.yml`. Desde la raíz del proyecto, ejecuta:
   ```bash
   docker-compose up -d
   ```
   *Esto levantará una instancia local de MongoDB en el puerto `27017`.*

2. **Ejecutar la API:**
   ```bash
   dotnet run --project src/GtMotive.Estimate.Microservice.Api
   ```
3. Accede a **Swagger** en `https://localhost:<puerto>/swagger` para interactuar con los endpoints.

## 🧪 Estrategia de Testing

El proyecto cuenta con una suite de pruebas exhaustiva que cubre los diferentes niveles de la pirámide de testing, garantizando la calidad y robustez del código. Se han configurado analizadores de código estrictos (*StyleCop*, *SonarAnalyzer*) para mantener un estándar de calidad altísimo.

Para ejecutar todas las pruebas de golpe:
```bash
dotnet test
```

### 1. Pruebas Unitarias de Dominio (`UnitTests/Domain`)
Validan las reglas de negocio intrínsecas (Invariantes) de los Agregados y Value Objects.
* **Características:** Ultrarrápidas, sin dependencias, ni Mocks.
* **Ejemplo:** Validar que un vehículo no puede ser instanciado si su fecha de fabricación (`ManufactureDate`) es superior a 5 años.

### 2. Pruebas Unitarias de Aplicación (`UnitTests/ApplicationCore`)
Validan el flujo de los Casos de Uso.
* **Características:** Aisladas completamente de la infraestructura mediante el uso de **Moq**.
* **Ejemplo:** Comprobar que `RentVehicleUseCase` devuelve un error si el cliente ya tiene un vehículo alquilado, sin llegar a consultar la base de datos.

### 3. Pruebas de Infraestructura - Host (`InfrastructureTests`)
Prueban el adaptador de entrada (Controladores) sin ejecutar la lógica de negocio real.
* **Características:** Levantan un `TestServer` en memoria. Se falsean los Casos de Uso para validar únicamente el enrutamiento HTTP y las validaciones de los modelos de entrada (`[JsonRequired]`).

### 4. Pruebas Funcionales (`FunctionalTests`)
Pruebas de integración de extremo a extremo excluyendo la capa HTTP (Host).
* **⚠️ IMPORTANTE:** Estas pruebas requieren que el contenedor de **MongoDB esté en ejecución** (`docker-compose up -d`).
* **Características:** Utilizan el contenedor de dependencias real (`CompositionRoot`) inyectando un presentador falso (Dummy) para validar que el Caso de Uso persiste correctamente los datos en la base de datos real de MongoDB.