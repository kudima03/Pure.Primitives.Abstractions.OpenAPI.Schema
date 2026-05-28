# Pure.Primitives.Abstractions.OpenAPI.Schema

OpenAPI document transformer for the **Pure** ecosystem — replaces abstract primitive interface schemas with native OpenAPI type representations.

[![.NET build & test](https://github.com/kudima03/Pure.Primitives.Abstractions.OpenAPI.Schema/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.Primitives.Abstractions.OpenAPI.Schema/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/Pure.Primitives.Abstractions.OpenAPI.Schema/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.Primitives.Abstractions.OpenAPI.Schema/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.Primitives.Abstractions.OpenAPI.Schema)](https://www.nuget.org/packages/Pure.Primitives.Abstractions.OpenAPI.Schema)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

When ASP.NET Core generates an OpenAPI document for controllers or minimal API endpoints whose parameters or return types implement [`Pure.Primitives.Abstractions`](https://github.com/kudima03/Pure.Primitives.Abstractions) interfaces (`IString`, `INumber<T>`, `IDate`, etc.), the resulting schema components appear as opaque `object` types instead of the underlying primitives.

`Pure.Primitives.Abstractions.OpenAPI.Schema` provides `PrimitivesDocumentTransformer` — an `IOpenApiDocumentTransformer` that post-processes the generated document and replaces those abstract schema entries with the correct native OpenAPI types, so API consumers see standard primitive types in the spec.

## API

### `PrimitivesDocumentTransformer`

A sealed, stateless `IOpenApiDocumentTransformer`. Register it once; it applies the following schema replacements to `document.Components.Schemas`:

| Schema name | OpenAPI type | Format |
|---|---|---|
| `IString` | `string` | — |
| `IBool` | `boolean` | — |
| `IChar` | `string` | *(maxLength: 1)* |
| `IGuid` | `string` | `uuid` |
| `IDate` | `string` | `date` |
| `ITime` | `string` | `time` |
| `IDateTime` | `string` | `date-time` |
| `IDayOfWeek` | `integer` | — |
| `INumberByte` / `INumberSByte` / `INumberInt16` / `INumberUInt16` / `INumberInt32` | `integer` | `int32` |
| `INumberUInt32` / `INumberInt64` / `INumberUInt64` / `INumberIntPtr` / `INumberUIntPtr` | `integer` | `int64` |
| `INumberSingle` | `number` | `float` |
| `INumberDouble` / `INumberDecimal` | `number` | `double` |

Schema names follow ASP.NET Core OpenAPI's convention of concatenating the interface base name with the CLR type-argument name (e.g. `INumber<int>` → `INumberInt32`). Schemas not in the table are left unchanged.

## Target Frameworks

- .NET 10

## Installation

```shell
dotnet add package Pure.Primitives.Abstractions.OpenAPI.Schema
```

## Usage

Register the transformer when configuring OpenAPI:

```csharp
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<PrimitivesDocumentTransformer>();
});
```

After this, any `IString`, `INumber<T>`, `IDate`, and other Pure primitive interfaces used in your endpoints will appear as their native OpenAPI equivalents in the generated spec.
