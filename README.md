<div align="center">
  <h1>Knowriva</h1>
  <p><strong>Give your learning a direction.</strong></p>
  <p>
    An API-first platform for organizing learning goals, tracking progress,
    and turning consistent effort into visible growth.
  </p>

  <p>
    <img src="https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white" alt=".NET 10" />
    <img src="https://img.shields.io/badge/ASP.NET_Core-10.0-512BD4?logo=dotnet&logoColor=white" alt="ASP.NET Core" />
    <img src="https://img.shields.io/badge/Architecture-Layered-0F766E" alt="Layered architecture" />
    <img src="https://img.shields.io/badge/Status-Foundation_stage-F59E0B" alt="Foundation stage" />
  </p>
</div>

## Overview

Knowriva is being built around a simple idea: learning becomes easier to manage when goals, subjects, resources, and progress are connected in one place.

The project currently contains the initial ASP.NET Core solution and its core architectural boundaries. The first application features will be introduced incrementally on top of this foundation.

## Current foundation

- ASP.NET Core API entry point.
- Separate Domain, Application, and Infrastructure projects.
- Explicit project references that preserve the dependency direction.
- Nullable reference types and implicit global usings enabled.
- Repository-level `.gitignore` for build output and local files.

## Architecture

```mermaid
flowchart LR
    API[Knowriva.Api\nHTTP entry point]
    APP[Knowriva.Application\nUse cases and orchestration]
    DOMAIN[Knowriva.Domain\nBusiness rules and models]
    INFRA[Knowriva.Infrastructure\nExternal implementations]

    API --> APP
    API --> INFRA
    APP --> DOMAIN
    INFRA --> APP

    classDef api fill:#4F46E5,color:#fff,stroke:#312E81,stroke-width:2px
    classDef app fill:#0891B2,color:#fff,stroke:#155E75,stroke-width:2px
    classDef domain fill:#16A34A,color:#fff,stroke:#166534,stroke-width:2px
    classDef infra fill:#EA580C,color:#fff,stroke:#9A3412,stroke-width:2px

    class API api
    class APP app
    class DOMAIN domain
    class INFRA infra
```

| Project | Responsibility |
| --- | --- |
| `Knowriva.Api` | Hosts the HTTP API and acts as the application composition root. |
| `Knowriva.Application` | Coordinates use cases and defines application-facing abstractions. |
| `Knowriva.Domain` | Contains business concepts and rules independent of infrastructure. |
| `Knowriva.Infrastructure` | Provides implementations for persistence and other external concerns. |

## Planned capabilities

The platform is intended to grow through focused increments, including:

- Learning goals and skill areas.
- Topics, resources, and personal notes.
- Progress entries and completion history.
- Review reminders and learning streaks.
- Authentication and personal workspaces.
- Progress summaries and useful analytics.

## Getting started

### Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run the API

```bash
dotnet build Knowriva.slnx
dotnet run --project src/Knowriva.Api/Knowriva.Api.csproj
```

The running URL is printed in the terminal. At the current foundation stage, the API exposes the starter root endpoint while the first feature is being developed.

## Repository structure

```text
Knowriva/
├── src/
│   ├── Knowriva.Api/
│   ├── Knowriva.Application/
│   ├── Knowriva.Domain/
│   └── Knowriva.Infrastructure/
├── Knowriva.slnx
├── .gitignore
└── README.md
```

Knowriva will evolve as new capabilities, tests, API contracts, and delivery workflows are added.
