# Opinionated Minimal API Template

A `dotnet new` template for building structured ASP.NET Minimal APIs with optional JWT and AOT support.

Template short name: `owebapi`

## Features

- Minimal API structure with feature-based organization
- JWT modes: `None`, `HS256`, `ES256`
- Optional `/.well-known/jwts.json` endpoint (ES256 + public key exposure)
- Built-in config sections for Authentication, Database, WebSockets, and Logging
- Optional AOT generation (`GenerateAot`)
- Conditional file generation based on selected options

## Install

From local source:

```bash
dotnet new install .\OpionatedWebApi
```

From NuGet package (after publish):

```bash
dotnet new install Lmedeiros.OpinionatedWebApi.Template
```

Verify installation:

```bash
dotnet new list owebapi
```

## Usage

Create a default project:

```bash
dotnet new owebapi -n MyApi
```

Create with options:

```bash
dotnet new owebapi -n MyApi \
  --JwtMode ES256 \
  --JwtExposePublicKey true \
  --LogToConsole true \
  --LogToFile true \
  --GenerateAot true
```

## Key options

- `--JwtMode`: `None | HS256 | ES256`
- `--JwtExposePublicKey`: `true | false`
- `--DbProvider`: `Sqlite | SqlServer | InMemory`
- `--WebSocketsEnabled`: `true | false`
- `--LogToConsole`: `true | false`
- `--LogToFile`: `true | false`
- `--GenerateAot`: `true | false`

## Uninstall

```bash
dotnet new uninstall Lmedeiros.OpinionatedWebApi.Template
```

