# Restaurant Hub

Sistema transaccional para la gestión de pedidos y pagos de un restaurante.

## Descripción

Restaurant Hub es un proyecto orientado al diseño e implementación de un sistema transaccional que permite organizar y gestionar información relacionada con productos, categorías, clientes, usuarios, pedidos, pagos e inventario de un restaurante.

El proyecto se desarrolla de manera progresiva, partiendo del prototipo de interfaz elaborado en la Fase 1 y continuando con la construcción de la base de datos y la estructura inicial del backend en la Fase 2.

## Estado del proyecto

Actualmente el proyecto se encuentra en construcción técnica.

Se ha implementado:

* La base de datos relacional del sistema (12 tablas), con claves primarias, foráneas, restricciones de integridad y datos de prueba.
* El backend en tres capas (controladores, servicios y repositorios) conectado a SQL Server mediante Entity Framework Core.
* Gestión de categorías y productos (crear, consultar, actualizar y desactivar).
* Registro transaccional de pedidos: cálculo automático de subtotales y totales, descuento de ingredientes según la receta de cada producto y reversión completa (rollback) si no hay inventario suficiente.
* Cambio de estado de los pedidos con transiciones válidas; al cancelar, los ingredientes vuelven al inventario.
* Consulta del inventario de ingredientes y generación automática de alertas de reabastecimiento.
* Consulta de clientes y usuarios para el registro de pedidos.
* Interfaz web responsive conectada a la API: inicio, nuevo pedido, pedidos, productos, categorías e inventario.

Quedan pendientes la autenticación y roles, el registro de clientes, pagos, facturación y la predicción de demanda.

## Tecnologías utilizadas

### Frontend

* HTML5, CSS3 y JavaScript
* Vue 3 (cargado desde CDN, sin instalación ni compilación)
* Bootstrap 5 y Bootstrap Icons (cargados desde CDN)

### Backend

* C#
* .NET 8
* ASP.NET Core Web API
* Entity Framework Core

### Base de datos

* Microsoft SQL Server

## Estructura del proyecto

RestaurantHub/
├── backend/
│   └── RestaurantHub.API/
│       ├── Controllers/    (capa de presentación: endpoints de la API)
│       ├── Services/       (capa de negocio: reglas y transacciones)
│       ├── Repositories/   (capa de acceso a datos)
│       ├── Data/           (DbContext de Entity Framework)
│       ├── Models/         (entidades que representan las tablas)
│       ├── DTOs/           (datos que recibe y devuelve la API)
│       ├── Exceptions/     (errores de negocio)
│       ├── Program.cs
│       ├── appsettings.json
│       └── RestaurantHub.API.csproj
│
├── database/
│   └── QueryDBRestaurant.sql
│   └── ScriptDBRestaurante.sql
│
├── frontend/
│   ├── index.html          (inicio)
│   ├── nuevo-pedido.html
│   ├── pedidos.html
│   ├── productos.html
│   ├── categorias.html
│   ├── inventario.html
│   ├── css/styles.css
│   └── js/
│       ├── config.js       (dirección de la API)
│       ├── api.js          (todas las llamadas a la API)
│       ├── componentes.js  (menú, ventanas emergentes, avisos y formatos)
│       └── paginas/        (la lógica de cada página)
│
└── README.md

## Base de datos

La base de datos utilizada por el proyecto se denomina:
`RestaurantHubDB`
Los archivos relacionados con la base de datos se encuentran en la carpeta:
database/

### QueryDBRestaurant.sql

Este archivo contiene el script utilizado para la creación y configuración de la base de datos durante el desarrollo.

Incluye:
* Creación de la base de datos `RestaurantHubDB`.
* Creación de las tablas del sistema.
* Definición de claves primarias y foráneas.
* Restricciones de integridad.
* Inserción de datos de prueba.
* Consultas de prueba para verificar la información almacenada.

### ScriptDBRestaurante.sql

Este archivo corresponde al script generado desde SQL Server Management Studio (SSMS) a partir de la base de datos creada.
Contiene la estructura de las tablas, claves primarias, claves foráneas, restricciones, índices y demás elementos de configuración generados por SQL Server.
Este archivo se conserva como una representación del esquema actual de la base de datos generado directamente desde SQL Server.

### Ejecución de la base de datos

Para crear y poblar la base de datos, abrir el archivo:

database/QueryDBRestaurant.sql

en SQL Server Management Studio (SSMS) y ejecutar el script.
El script crea la base de datos RestaurantHubDB, sus tablas, relaciones, restricciones y datos iniciales de prueba.

El archivo ScriptDBRestaurante.sql corresponde al script generado desde SQL Server Management Studio (SSMS) y se conserva como representación del esquema actual de la base de datos.

## Backend

El backend se encuentra en:
backend/RestaurantHub.API

Está desarrollado utilizando ASP.NET Core Web API sobre .NET 8.

Para ejecutar el backend, abrir una terminal dentro de la carpeta:
backend/RestaurantHub.API

y ejecutar:
dotnet run

El proyecto se ejecutará localmente y mostrará en la terminal la dirección en la que se encuentra disponible la aplicación.

## Conexión con la base de datos

El backend utiliza Entity Framework Core para establecer la conexión con SQL Server.

La cadena de conexión se encuentra configurada en:
backend/RestaurantHub.API/appsettings.json

La configuración utilizada actualmente apunta a la base de datos:
RestaurantHubDB

en la instancia local de SQL Server.

La conexión fue comprobada desde el backend mediante Entity Framework Core, obteniendo una conexión exitosa con la base de datos.

## Frontend

La interfaz se encuentra en la carpeta `frontend/`. Está hecha con Vue 3 y Bootstrap 5 cargados desde CDN, por lo que no requiere instalar dependencias ni compilar: se necesita conexión a internet la primera vez que se abre.

Cada página HTML carga su propio archivo de `js/paginas/`. Todas usan `js/api.js` para comunicarse con el backend y `js/componentes.js` para el menú lateral, las ventanas emergentes y los avisos.

| Página | Qué permite |
|---|---|
| `index.html` | Resumen con conteos reales, pedidos por estado, pedidos recientes y alertas |
| `nuevo-pedido.html` | Elegir productos disponibles, ver el total y registrar el pedido |
| `pedidos.html` | Historial con filtros, detalle del pedido y cambio de estado |
| `productos.html` | Catálogo con filtros; crear, editar, desactivar y ver la receta |
| `categorias.html` | Crear, editar y eliminar categorías |
| `inventario.html` | Existencias de ingredientes y alertas de reabastecimiento |

La dirección de la API está en `frontend/js/config.js` (`http://localhost:5127/api`). Si el backend se ejecuta en otro puerto, se cambia ahí.

## Ejecución general del proyecto

Para trabajar con el proyecto:

1. Ejecutar el script `database/QueryDBRestaurant.sql` en SQL Server Management Studio.
2. Verificar que la base de datos `RestaurantHubDB` haya sido creada correctamente.
3. Abrir una terminal en `backend/RestaurantHub.API`.
4. Ejecutar en powerShell:
    dotnet run

5. Abrir en el navegador la dirección que muestra la terminal seguida de `/swagger` (por ejemplo `http://localhost:5127/swagger`) para probar los endpoints.
6. Abrir `frontend/index.html` en el navegador (doble clic o con la extensión Live Server de VS Code) para usar la interfaz. El backend debe estar en ejecución en `http://localhost:5127`.

## Endpoints disponibles

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/categorias` | Lista las categorías |
| GET | `/api/categorias/{id}` | Consulta una categoría |
| POST | `/api/categorias` | Crea una categoría (nombre único) |
| PUT | `/api/categorias/{id}` | Actualiza una categoría |
| DELETE | `/api/categorias/{id}` | Elimina una categoría sin productos asociados |
| GET | `/api/productos?nombre=&idCategoria=&disponible=` | Catálogo de productos con filtros |
| GET | `/api/productos/{id}` | Consulta un producto con su receta |
| POST | `/api/productos` | Crea un producto |
| PUT | `/api/productos/{id}` | Actualiza un producto |
| DELETE | `/api/productos/{id}` | Desactiva un producto (no lo borra) |
| GET | `/api/pedidos?estado=&desde=&hasta=` | Historial de pedidos con filtros |
| GET | `/api/pedidos/{id}` | Consulta un pedido con sus detalles |
| POST | `/api/pedidos` | Registra un pedido (transacción con descuento de inventario) |
| PATCH | `/api/pedidos/{id}/estado` | Cambia el estado: PENDIENTE → PREPARANDO → ENTREGADO, o CANCELADO |
| GET | `/api/ingredientes?bajoMinimo=` | Inventario de ingredientes |
| GET | `/api/ingredientes/alertas` | Alertas de reabastecimiento pendientes |
| GET | `/api/clientes` | Lista los clientes (para el registro de pedidos) |
| GET | `/api/usuarios` | Lista los usuarios, sin contraseña (para el registro de pedidos) |

El archivo `backend/RestaurantHub.API/RestaurantHub.API.http` contiene peticiones de ejemplo que se pueden ejecutar desde Visual Studio.

## Alcance de la fase actual

Esta fase implementa el núcleo transaccional del sistema: la gestión del catálogo y el registro de pedidos con control de inventario por recetas, junto con la interfaz web que consume la API.

La estructura desarrollada servirá como base para implementar posteriormente la autenticación, clientes, pagos, facturación y predicción de demanda.
