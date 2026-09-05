# Changelog

All notable changes to Pure.Primitives.Abstractions.OpenAPI.Schema are documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [0.1.0-preview.0.2.1] — 2026-08-21

- Maintenance release: dependency and build updates.

---

## [0.1.0-preview.0.2.0] — 2026-08-13

- Maintenance release: dependency and build updates.

---

## [0.1.0-preview.0.1.2] — 2026-08-06

### Fixed

- Pinned the `Microsoft.OpenApi` dependency to `2.11.0` directly, resolving a
  known vulnerability (GHSA-v5pm-xwqc-g5wc / NU1903) that was otherwise
  pulled in transitively through `Microsoft.AspNetCore.OpenApi`.

---

## [0.1.0-preview.0.1.1] — 2026-06-10

- Maintenance release: dependency and build updates.

---

## [0.1.0-preview.0.1.0] — 2026-05-13

### Added

- **`PrimitivesDocumentTransformer`** — an `IOpenApiDocumentTransformer`
  implementation that rewrites `Pure.Primitives.Abstractions` interface
  schemas (`IString`, `IBool`, `IGuid`, `IChar`, `IDate`, `ITime`,
  `IDateTime`, `IDayOfWeek`, `INumber<T>` for all supported numeric types)
  into their native OpenAPI schema equivalents, so API consumers see plain
  primitive types instead of the library's wrapper interfaces.
