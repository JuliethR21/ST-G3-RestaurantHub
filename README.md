# Restaurant Hub

Sistema transaccional para la gestión de pedidos y pagos de un restaurante.

## Descripción

Restaurant Hub es un proyecto orientado al diseño e implementación de un sistema transaccional que permite organizar y gestionar información relacionada con productos, categorías, clientes, usuarios, pedidos, pagos e inventario de un restaurante.

El proyecto se desarrolla de manera progresiva, partiendo del prototipo de interfaz elaborado en la Fase 1 y continuando con la construcción de la base de datos y la estructura inicial del backend en la Fase 2.

## Estado del proyecto

Actualmente el proyecto se encuentra en la etapa inicial de construcción técnica.

En esta fase se implementaron:

* La base de datos relacional del sistema.
* Las tablas principales y sus relaciones.
* Claves primarias y claves foráneas.
* Restricciones básicas de integridad.
* Datos iniciales de prueba.
* La estructura inicial del backend.
* La conexión del backend con la base de datos SQL Server.

Las operaciones CRUD y la integración completa de la interfaz con el backend se desarrollarán en etapas posteriores.

## Tecnologías utilizadas

### Frontend

* HTML5
* CSS3
* JavaScript

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
│       ├── Controllers/
│       ├── Data/
│       ├── Models/
│       ├── Services/
│       ├── Program.cs
│       ├── appsettings.json
│       └── RestaurantHub.API.csproj
│
├── css/
│   └── styles.css
│
├── database/
│   └── QueryDBRestaurant.sql
│   └── ScriptDBRestaurante.sql
│
├── index.html
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

## Prototipo de interfaz

El prototipo inicial de la interfaz se encuentra en:
index.html

Los estilos utilizados por la interfaz se encuentran en:
css/styles.css

## Ejecución general del proyecto

Para trabajar con el proyecto:

1. Ejecutar el script `database/ScriptDBRestaurante.sql` en SQL Server Management Studio.
2. Verificar que la base de datos `RestaurantHubDB` haya sido creada correctamente.
3. Abrir una terminal en `backend/RestaurantHub.API`.
4. Ejecutar en powerShell:
    dotnet run

5. Abrir el archivo `index.html` para visualizar el prototipo de la interfaz.

## Alcance de la fase actual

Esta fase tiene como propósito establecer la estructura técnica inicial del sistema. Por esta razón, todavía no se encuentran implementadas todas las operaciones transaccionales ni la integración completa entre la interfaz y el backend.

La estructura desarrollada servirá como base para implementar posteriormente las operaciones de gestión de productos, clientes, pedidos, pagos e inventario.
