# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet test --no-build                                # run xUnit tests
dotnet format --verify-no-changes                     # check code style (CI enforces this)
dotnet format && csharpier format .                   # auto-fix code style
dotnet pack --configuration Release -p:PackageVersion=<version> --output .
```

## Architecture

This is a **single-class NuGet library** — one public type, no state, no configuration.

**`PrimitivesDocumentTransformer`** (sealed) implements `IOpenApiDocumentTransformer` from `Microsoft.AspNetCore.OpenApi`. It holds a static `IReadOnlyDictionary<string, OpenApiSchema>` mapping Pure.Primitives.Abstractions interface names (as ASP.NET Core OpenAPI renders them in `document.Components.Schemas`) to their native OpenAPI schema equivalents. `TransformAsync` iterates the dictionary and replaces any matching schema entry; it returns early if `Components` or `Schemas` is null.

The library itself does **not** reference `Pure.Primitives.Abstractions` as a NuGet dependency — it only knows the string schema names that ASP.NET Core emits when it reflects over those interfaces.

**Test project** (`src/Tests/`): xUnit, targets net10.0. Tests directly instantiate `PrimitivesDocumentTransformer`, pass hand-crafted `OpenApiDocument` instances, and assert the resulting schema type and format.

**Package validation:** `EnablePackageValidation = true` with `PackageValidationBaselineVersion = 0.1.0-preview.0.1.0`. Breaking changes fail the build.

**AOT:** `IsAotCompatible = true`. Keep all new code AOT-safe (no reflection over unknown types, no `dynamic`).

**Publishing:** triggered by pushing a semver tag (`*.*.*`). The tag value becomes `PackageVersion`.

## Code Style

Enforced via `.editorconfig`, `dotnet format`, and CSharpier:

- No `var` — always use explicit types.
- No expression-bodied methods, constructors, operators, or local functions. Expression-bodied properties, indexers, accessors, and lambdas are required.
- Private fields: `_camelCase` (underscore prefix).
- No non-private instance fields.
- File-scoped namespaces (`namespace Foo;` not `namespace Foo { }`).
- Max line length: 90 characters.
- `using` directives outside the namespace, `System.*` sorted first.
- `new()` target-typed creation is disallowed when the type is apparent — use `new FullTypeName(...)`.
- All braces on their own line (`csharp_new_line_before_open_brace = all`).

## Commit Messages

Do not mention Claude or AI assistance in commit messages.
