# Lippi-Homework

## Resumen
Este proyecto consiste en una API RESTful para gestionar productos. Se ha utilizado .NET 8.0 y Entity Framework Core, se ha implementado un CRUD sobre los productos que se encuentran almacenados en una base de datos SQL Server. Además, se han creado pruebas unitarias y de integración para asegurar el correcto funcionamiento de la API.

## Tecnologías Utilizadas
- **.NET 8.0**: Framework para construir aplicaciones web.
- **Entity Framework Core**: ORM para interactuar con bases de datos.
- **InMemory Database**: Base de datos en memoria para pruebas.
- **Nunit**: Framework para realizar pruebas unitarias.
- **Xunit**: Framework para realizar pruebas de integración.
- **Swagger**: Herramienta para documentar y probar la API.

## Como ejecutar el proyecto:

- **Restaurar dependencias**: 
  ```
  dotnet restore
  ```
- **Ejecutar la aplicación**:
  ```
  dotnet run
  ```
- **Acceder a la documentacion**:
  ```
  http://localhost:5287/swagger
  ```
- **Ejecutar pruebas unitarias y de integracion**:
  ```
  dotnet test
  ```

# Prueba de Evaluación:
## Creación de una API REST con C# y .NET Core
## Descripción
Esta prueba tiene como objetivo evaluar tus conocimientos y habilidades en la creación de una API REST utilizando C# y .NET Core.
Deberás implementar una API para gestionar un inventario de productos, incluyendo validación de datos, pruebas y documentación.
## Requisitos
- Funcionalidad de la API
## Modelo de Producto:
- Id (int): Identificador único del producto.
- Nombre (string): Nombre del producto (no vacío).
- Precio (decimal): Precio del producto (valor positivo).
- Stock (int): Cantidad en inventario.
## Endpoints:
- GET /api/productos: Obtener todos los productos.
- GET /api/productos: Obtener un producto por Id, nombre.
- GET /api/productos/{id}: Obtener un producto por Id.
- POST /api/productos: Crear un nuevo producto.
- PUT /api/productos/{id}: Actualizar un producto existente.
- DELETE /api/productos/{id}: Eliminar un producto.
## Validación:
Validar que el nombre del producto no esté vacío. Validar que el precio sea un valor positivo.
## Pruebas:
Incluir pruebas unitarias para los controladores y servicios. Incluir pruebas de integración para verificar el funcionamiento completo de la API.
## Documentación:
Documentar la API utilizando Swagger/OpenAPI.
## Instrucciones
- Configuración del Proyecto: Crea un nuevo proyecto de API en .NET 8. Añade las dependencias necesarias, validación de datos, y Swagger.
- Implementación: Define el modelo de datos para los productos (adjunta el .bak de dicho modelo). Implementa los controladores y servicios necesarios para manejar las solicitudes HTTP.
- Añadir validaciones de datos y manejo de errores.
## Pruebas:
Crea pruebas unitarias para los métodos de los controladores y servicios.
Crea pruebas de integración para verificar el funcionamiento de la API en conjunto.
## Documentación:
Configura Swagger para generar la documentación de la API.
Asegúrate de que todos los endpoints estén documentados y que la documentación sea clara y completa.
## Entrega:
Sube el código a un repositorio en GitHub.
Incluye este archivo README en el repositorio.
Proporciona instrucciones claras sobre cómo ejecutar la API y las pruebas.
## Evaluación La evaluación se basará en los siguientes criterios:
Correcta implementación de los endpoints y funcionalidades requeridas.
Calidad del código y buenas prácticas de desarrollo.
Efectividad de las pruebas unitarias y de integración.
Completitud y claridad de la documentación.
## ¡Buena suerte!
