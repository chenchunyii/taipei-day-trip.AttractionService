# [Taipei Day Trip Backend Service](https://chun-web-api.online/api/attraction/swagger/index.html)

*This repository hosts the backend service for the "Taipei Day Trip" application. Developed with ASP.NET Core 8, it utilizes a **Three-Layer Architecture** to organize the codebase. It provides the necessary API endpoints to support front-end application functionalities and is deployed within an AWS cloud environment.*

---

## Project Features

* **Tech Stack:** .NET 8
* **Architecture:** Implements a **Three-Layer Architecture**, clearly separating the **Controller**, **Service**, and **Repository** layers. This design enhances the project's maintainability and extensibility.
* **ORM / Data Access:** Combines **Dapper** and **Entity Framework Core (EF Core)** for database operations.
* **API Documentation:** Integrates **Swagger/OpenAPI** to provide an automated API documentation interface, making it easy for developers to test and understand the APIs.
* **Database:** Supports **MySQL database** connectivity via Pomelo.EntityFrameworkCore.MySql.

---

## Deployment & Infrastructure

This project is deployed on AWS cloud services and utilizes Docker for containerization.

* **AWS Services:**
    * **AWS EC2 (Elastic Compute Cloud):** Serves as the primary host machine for the backend application, providing scalable computing capacity.
    * **AWS RDS (Relational Database Service):** Offers a managed MySQL relational database service for storing all application data, ensuring high availability and reliability.
    * **AWS Route 53:** Manages DNS records (including CNAME and A records) for the domain, enabling accurate mapping of domain names to application services.
* **Deployment Management:**
    * **Docker Compose:** Used to define and run multi-container Docker applications.
* **Reverse Proxy:**
    * **Nginx:** Acts as a reverse proxy server, responsible for forwarding external requests to the backend application.

---

## Technologies and Packages

This project leverages the following key NuGet packages:

* **AutoMapper.Extensions.Microsoft.DependencyInjection (12.0.1):** Simplifies object-to-object mapping, commonly used for DTO (Data Transfer Object) and domain model conversions.
* **Dapper (2.1.66):** A lightweight Object Relational Mapper (ORM) that offers high-performance database operations.
* **Microsoft.AspNetCore.OpenApi (8.0.15):** Integrates OpenAPI (Swagger) capabilities into the ASP.NET Core application.
* **Microsoft.EntityFrameworkCore.Tools (8.0.0):** Provides EF Core command-line tools for managing database migrations and other tasks.
* **Pomelo.EntityFrameworkCore.MySql (8.0.0):** Enables Entity Framework Core to interact with MySQL databases.
* **Swashbuckle.AspNetCore (6.6.2):** Implements Swagger/OpenAPI document generation and UI services.

---

## Third-Party Integrations

* **TapPay:** Integrated for payment processing, handling secure online payment transactions within the application.

---

## Version Control

* **Git/GitHub:** Uses Git for distributed version control and GitHub as the platform for code hosting and version management.

---

## Swagger API
* **URL:** [https://chun-web-api.online/api/attraction/swagger/index.html](https://chun-web-api.online/api/attraction/swagger/index.html)

## Contact
- Author: `Chen, Chun-Yi`
- Email: `chun.yii.chen@gmail.com`
# taipei-day-trip.AttractionService
