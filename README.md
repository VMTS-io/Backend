# VeeManage

## Overview

This repository contains the backend service for the Vehicle Management and Tracking System (VMTS), a comprehensive solution for managing vehicle fleets. Built with .NET 9 and ASP.NET Core, it provides a robust API for handling vehicles, users, trips, maintenance schedules, and reporting. The system is designed with a clean architecture, incorporating modern technologies for real-time communication, background job processing, and AI-powered predictions.

## Key Features

- **User & Role Management**: JWT-based authentication and role-based authorization for Admins, Managers, Drivers, and Mechanics.
- **Vehicle Management**: Full CRUD operations for vehicles, models, and categories. Includes support for creating vehicles with maintenance history from Excel spreadsheets.
- **Trip & Fault Reporting**:
  - Manage trip requests, including daily recurring trips.
  - Drivers can submit trip reports (fuel, cost) and fault reports.
- **Real-time Location Tracking**: Utilizes SignalR and Redis to broadcast and persist live vehicle locations during trips.
- **Comprehensive Maintenance Workflow**:
  - Create and manage maintenance requests.
  - Mechanics can submit initial diagnostic and final repair reports.
  - Tracks parts inventory and costs associated with repairs.
- **AI & ML Integration**:
  - **Fault Prediction**: Predicts the type and urgency of a fault from a driver's textual report.
  - **Maintenance Prediction**: Predicts when a vehicle requires maintenance based on its history and usage patterns.
  - **Odometer OCR**: Extracts odometer readings directly from uploaded images.
- **Background Jobs with Hangfire**:
  - Automatically create daily recurring trips.
  - Periodically recalculate maintenance due dates and kilometers.
  - Run batch maintenance predictions for the entire fleet.
- **Dashboard & Analytics**: Endpoints to power a dashboard with key metrics like total trips, maintenance costs, fuel costs, and vehicle availability.

## Tech Stack

- **Framework**: .NET 9, ASP.NET Core Web API
- **Database**: MS SQL Server with Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Real-time**: SignalR, Redis
- **Background Jobs**: Hangfire
- **Architecture**: Clean Architecture (Core, Repository, Service, API layers)
- **API Documentation**: Scalar / OpenAPI
- **Containerization**: Docker, Docker Compose
- **Tooling**: AutoMapper, FluentValidation, EPPlus (for Excel)

## Project Structure

The solution is organized into four main projects, following Clean Architecture principles:

- `VMTS.API`: The main entry point of the application. Contains Controllers, DTOs, middleware, API configuration, and the Dockerfile.
- `VMTS.Core`: The core of the application. Contains all domain entities, business logic interfaces (services, repositories), specifications, and cross-cutting helpers. It has no dependencies on other layers.
- `VMTS.Repository`: The data access layer. Implements the Repository and Unit of Work patterns using Entity Framework Core to interact with the database.
- `VMTS.Service`: The service layer that orchestrates business logic. It contains implementations for services, integrations with external APIs (like AI models), and Hangfire job definitions.

## Getting Started

### Prerequisites

- .NET 9 SDK
- Docker Desktop

### Running with Docker

The easiest way to get the application and its database running is by using Docker Compose.

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/vmts-io/backend.git
    cd backend
    ```

2.  **Run Docker Compose:**
    From the root directory of the repository, execute the following command:

    ```bash
    docker-compose up --build
    ```

    This command will:

    - Build the Docker image for the .NET application.
    - Pull the `mcr.microsoft.com/mssql/server:2022-latest` image for the database.
    - Create and start containers for both the application (`asp-app`) and the SQL Server database (`sql-server`).

3.  **Access the API:**
    - The API will be available at `http://localhost:8080` (HTTP) and `https://localhost:8081` (HTTPS).
    - API documentation is available via Scalar at `http://localhost:8080/scalar/v1`.

### Database and Seeding

The application is configured to automatically apply database migrations and seed initial data on startup. This includes creating roles (Admin, Manager, etc.) and seeding sample users, vehicles, and parts from the JSON files located in `src/VMTS.Repository/Data/DataSeed/`.

## API Documentation

This project uses Scalar for beautiful, interactive API documentation. Once the application is running, navigate to `/scalar/v1` in your browser.

The Scalar UI allows you to explore all available endpoints, view schemas, and execute API requests directly. Test tokens for different user roles are provided in `src/VMTS.API/appsettings.json` and are pre-configured in the Scalar authentication settings for convenience.
