

# AspNetCore.EventSourcing

A simple ASP.NET Core **Event Sourcing** solution using **Clean Architecture** and **Domain-Driven Design** principles.

The solution keeps things simple by using a single **SQL Server** database for Event Streams (Write models) and Projections (Read models). **Entity Framework Core** is used as an ORM across the application. **MediatR** is used for **CQRS** and Event publishing.

The setup follows important modern development principles such as high test coverage, SOLID principles, containerisation, code-first database management, enforced code styles, API tests and architecture tests.

The application uses a simple **Banking** example with 2 Bounded Contexts:

- **Customers**: Simple Entities are used to store Customers. Event Sourcing is complex and should only be used if  your requirements really need it.
- **Accounts**: Accounts are much more complex and involve different types of Transactions. Event Sourcing is a great fit here so we can keep track of all of the different Transactions that occur on an Account over time. There is an Event Sourced **Account** Write Model, with 2 different Projections: **AccountReadModel** and **TransactionReadModel**.

The .NET project contains the following components:

- **API** - ASP.NET 7 REST API with Swagger support
- **Database** - SQL Server database integration via Entity Framework Core
- **Migrations** - Code-First database migrations managed using a console application

## Table of Contents

- [Clean Architecture](#Clean-Architecture)
  - [Table of Contents](#Table-of-Contents)
  - [Quick Start](#Quick-Start)
- [Developer Guide](#Developer-Guide)
  - [IDE](#ide)
  - [Solution Project Structure](#Solution-Project-Structure)
  - [Nuget Libraries](#Nuget-Libraries)
  - [Entity Framework Core](#Entity-Framework-Core)
  - [Docker](#docker)
  - [Kubernetes](#kubernetes)
- [Architecture and Development Principles](#Architecture-and-Development-Principles)
  - [Clean Architecture](#Clean-Architecture)
  - [Domain-Centric Architecture](#Domain-Centric-Architecture)
  - [Domain-Driven Design](#Domain-Driven-Design)
  - [Command Query Responsibility Segregation](#Command-Query-Responsibility-Segregation)
  - [Domain Events](#Domain-Events)
  - [Read Model Projections](#Read-Model-Projections)
- [Migrations](#Migrations)
  - [Create a Database Migration](#Create-a-Database-Migration)
  - [Run Migrations](#Run-Migrations)
- [Running the Project](#Running-the-Project)
  - [Local Environment Setup](#Local-Environment-Setup)
  - [Projects to Run](#Projects-to-Run)
- [Testing](#testing)
  - [Unit Testing](#unit-testing)
  - [API Tests](#api-tests)
  - [Architecture Tests](#Architecture-Tests)
  

## Quick Start

*All of the required tools and libraries must be installed using the **Developer Guide** before these steps can be run.*

1. Start local database in Docker

```bash
docker-compose --profile dev up -d
```

2. Run the **AspNetCore.EventSourcing.Migrations** project to deploy database schema
3. Run the **AspNetCore.EventSourcing.Api** project to debug the application


# Developer Guide

The following steps are required to get your Developer machine ready for working with this project. 

This project uses NET 7.0 and Angular. Everything you need to get started should be included below - if there are any gaps, please do add them in.

## IDE

### Visual Studio 2022

Visual Studio 2022 is recommending for opening the main solution for this project - `AspNetCore.EventSourcing.sln`.

### Visual Studio Code

VS Code is recommended for working on the Angular SPA application due to the additional extensions that can be used.

## Solution Project Structure

The solution is broken down into the following projects:

- **AspNetCore.EventSourcing.Api** - ASP.NET 7 Web API with Swagger support
- **AspNetCore.EventSourcing.Application** - Application layer containing Commands/Queries/Domain Event Handlers/Read Model Handlers
- **AspNetCore.EventSourcing.Core** - Domain layer containing Event Sourced Aggregates, Entities and Domain Events
- **AspNetCore.EventSourcing.Infrastructure** - Infrastructure layer for all external integration e.g. database, notifications, serialization
- **AspNetCore.EventSourcing.Hosting** - Hosting cross-cutting concerns e.g. configuration and logging
- **AspNetCore.EventSourcing.Migrations** - Code-First EF database migrations and migration runner

### Test Projects

Each source project has a relevant test project for Unit/API tests.

## Nuget Libraries

The following Nuget libraries are used across the solution:

- **Entity Framework Core (EF Core)** - ORM for interacting with Database. *SQL Server* provider used by default however EF makes it easy to swap out for a different database.
- **Autofac** - Used for dependency injection and splitting service registration into 'Modules'
- **Swagger/Swashbuckle** - UI for interacting with API
- **AutoMapper** - Used to map from Entities in *Core* to DTOs in *Application*
- **MediatR** - Mediator implementation for implementing *Domain Event Handlers* and *CQRS*
- **CSharpFunctionalExtensions** - Base Class implementation for *Entities* and *ValueObjects*
- **XUnit** - Test runner for Unit and API tests
- **FluentAssertions** - Fluent extension methods for running assertions in tests
- **Moq** - Test Doubles library for creating *Mocks* and *Stubs* in tests
- **NetArchTest** - Architecture testing library


## Entity Framework Core

Entity Framework Core (EF) is used as the database ORM for this proect.

Install the Entity Framework CLI
```bash
dotnet tool install --global dotnet-ef
```

## Docker

Docker is used to build and deploy the code for this project. It is also used to spin-up services for local development such as the database.

### Docker Desktop

Docker Desktop is recommended for running Docker if you are using a Windows development machine.

### Docker Compose

Docker Compose is used to orchestrate running Docker images for local development. This is included as part of Docker Desktop.

## Kubernetes

Kubernetes manifests have been created for deploying the application to Kubernetes if required.

### Helm

Helm can be used as a package manager to deploy the applications in this project to Kubernetes.

The Helm **charts** for this project can be found in the `charts` folder.

# Architecture and Development Principles

The following principles are used in the code and architecture for this application.

## Clean Architecture

Clean Architecture is used to split the projects used into the following layers:

- Presentation - API project
- Application - Application services
- Core - Domain logic and Entities
- Infrastucture - Services for working with external systems e.g. database, event bus

Clean architecture puts the business logic and application model at the center of the app. Instead of having business logic depend on data access or other infrastructure concerns, this dependency is inverted: infrastructure and implementation details depend on the Application Core. This is achieved by defining abstractions, or interfaces, in the Core and Application layers, which are then implemented by types defined in the Infrastructure layer. A common way of visualizing this architecture is to use a series of concentric circles, similar to an onion.

## Domain-Centric Architecture

Domain Centric Architecture is used to split the code in the Core and Application layer by use-cases e.g. Accounts, Customers.

The code within each use-case folder is split by object type e.g. Entities, Services.

Domain Centric Architecture makes it easier to navigate between code when working on a particular concept. It also means that code is already separated for if a concept needs to be moved in the future such as to its own independent Microservice.

## Domain-Driven Design

Domain-Driven Design (DDD) is used to model the Core (Domain) layer of the app. The main data structures (Aggregate Roots) for the application are fully encapsulated by using private setters on any properties. By using this principle all Domain logic is encapsulated and controlled from within the Aggregate Root.

Data can only be loaded and saved using an Aggregate Root - therefore the IRepository interface has a generic contstraint for only working with the AggregateRoot base class.

Aggregates can work in coordination by emitting Domain Events which are subscribed to by Domain Event Handlers in a different Bounded Context. MediatR is used to dispatch the Domain Events to their associated Domain Event Handlers.

A full guide to the DDD techniques used in this solution can be found here:
https://betterprogramming.pub/domain-driven-design-a-walkthrough-of-building-an-aggregate-c84113aa9975

## Command Query Responsibility Segregation

CQRS is used within the Application layer to mediate data between the database and the API. The code is split into:

### Queries
Queries are used to retrieve data and should never mutate any state at all. Queries implement the base **Query** class.

Each Query class has an associated **QueryHandler**. 

### Commands
Commands implement the base **Command** or **CreateCommand** class and can be used to mutate state e.g. Create, Update, Delete.

CreateCommands should be used when creating data as they allow the Id of the created object to be returned.

Each Command class has an associated **CommandHandler** or **CreateCommandHandler** class. 

### MediatR

The MediatR Nuget package is used for the mediator implementation to match Queries/Commands/Domain Events to the associated handlers. The handlers are dynamically registered into the IoC container automatically.

Handlers are kept in the same file as their Query/Command. This makes it easier to navigate to them when debugging code.

## Domain Events

Domain Events are matched to their relevant Domain Event Handlers using MediatR. Domain Events are published via Aggregate Roots and dispatched directly before changes are saved to the database.

A full guide to the dispatching technique can be found here:
https://betterprogramming.pub/domain-driven-design-domain-events-and-integration-events-in-net-5a2a58884aaa

## Read Model Projections

Each Read Model has a ReadModelHandler in the Application layer to subscribe to relevant Domain Events for generating and updating the Read Model.


# Migrations

All schema and data migrations are automated using the **AspNetCore.EventSourcing.Migrations** project.

The migrations are generated from and held in the **AspNetCore.EventSourcing.Migrations** project. 
Migrations are generated automatically using the EF CLI. Each time the EF CLI is used a migration class will be added to a **Migrations** folder in the project. EF will compare the entities in your EF Context class to the existing snapshot and work out what migrations need to be added.

## Create a Database Migration

A migration can be generated using the following command:

```bash
dotnet ef migrations add <migration-name>
```

Before migrations can be added the database must be running. See 'Run Local Database in Docker'.

## Run Migrations

Migrations can be run by running the **AspNetCore.EventSourcing.Migrations** console app project.

# Running the Project

Before the project can be run all of the **Developer Setup** steps must be completed.

There are also some additional steps which must be run to setup your local environment.

## Local Environment Setup

The following steps must be performed to setup your local environment for running the project.

### Start Local Instances

Docker is used to start the following services locally:

- SQL Server

A **dev** profile is included in the docker-compose file to run the required local services:
```
docker-compose --profile dev up -d
```

The services can be stopped using:

```
docker-compose --profile dev down
```

This will not delete the data volumes created for the database so that the container can be started-up up again with the same data. To delete the data volume the following must be used:

```
docker-compose --profile dev down -v
```

### Setup Database

Once the local database is running in Docker the **AspNetCore.EventSourcing.Migrations** project (See [Run Migrations]) must be run to instantiate the database and populate some seed data.

## Projects to Run

Before running the application your local services must be running and you must setup your database using the steps above.

The following projects must be set as startup projects to run the entire solution:
- **AspNetCore.EventSourcing.Api**

# Testing

Various types of tests are used in the project. Each project being tested from the `src` folder has an associated project in the `tests` folder.

## Unit Testing

Unit tests should be written to test all code added to the Core and Infrastructure layers of the solution. Code in the Core (Domain) layer should have 100% code coverage without exception as it is the most important part of the application.

The **Moq** Nuget package is used to create Mocks and Stubs for the tests.

The **FluentAssertions** Nuget package is used to run assertions for the tests.

The **Builder Pattern** is used to build test Entities for the tests. This makes the tests more flexible and easier to change when the Entities change.

## API Tests

**WebApplicationFactory** is used to spin-up the API locally for testing. This allows services from the IoC container to be swapped out and mocked.

The API tests should test the *API* and *Application* layers of the solution. Any *Infrastructure* services should be mocked and swapped out. The **MockRepositoryFactory** class can be used to mock the **IRepository** interface.

## Architecture Tests

Tests are performed on the architecture of the solution using **NetArchTest**. These tests check that the Clean Architecture layering is adhered to and that the development principles and naming conventions being used are not breached.
