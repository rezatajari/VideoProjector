# Domain Capability Guide

## Purpose

This documentation describes the business capabilities and technical behavior implemented by the current `Domain` project. It is intended for developers, analysts, testers, and maintainers who need to understand what the domain model permits, rejects, calculates, and protects.

The guide is based on the source code in [`Domain`](../../Domain/) and documents the model as it exists now. It does not treat planned behavior as implemented behavior.

## Important Runtime Integration Status

The solution currently contains two different business-model layers:

1. The newer rich domain model in `Domain`, documented here.
2. The legacy data models in `Shared/Models`, which are still used by the API, Entity Framework Core configurations, controllers, and client contracts.

`API/API.csproj` currently references `Shared/Shared.csproj`, but it does not reference `Domain/Domain.csproj`. Therefore:

- The rules described in these guides are implemented and compilable in the `Domain` project.
- They are not automatically enforced by the currently running API or UI.
- Integrating these capabilities into runtime workflows requires the API/application layer and persistence mappings to use the `Domain` entities.
- API endpoint availability must not be inferred only from a public domain method.

This distinction is especially important for order status transitions, same-currency enforcement, protected line-total calculation, typed user data, and role assignment rules.

## Entity Guides

| Entity | Business responsibility | Guide |
|---|---|---|
| `Category` | Gives products a validated classification identity | [Category](Category.md) |
| `Product` | Represents catalog content and independent sale/rental offerings | [Product](Product.md) |
| `Role` | Defines a validated authorization role identity | [Role](Role.md) |
| `User` | Represents a customer or operator and manages role memberships | [User](User.md) |
| `Order` | Owns the shopping transaction, line items, total, and lifecycle | [Order](Order.md) |
| `OrderItem` | Captures one sale or rental line and calculates its line total | [OrderItem](OrderItem.md) |

## Business Capability Map

### Catalog management

- Create categories with normalized, non-empty names.
- Create products with typed names and optional descriptions.
- Assign or remove a product category reference.
- Enable or disable a product for direct sale.
- Enable or disable a product for daily rental.
- Keep separate sale and rental prices and stock quantities.
- Store an image URL and an optional test-video URL.

### Identity and access modeling

- Create users from validated full-name, email, password-hash, and phone-number values.
- Create roles with normalized, non-empty names.
- Add multiple role IDs to a user.
- Prevent duplicate user-role membership.
- Remove existing role membership and reject invalid removals.

### Ordering

- Create an order for a specific user.
- Add sale and rental items to a pending order.
- Reject rental dates that begin before an application-supplied current date.
- Calculate sale totals by unit price and quantity.
- Calculate rental totals by daily price, quantity, and billable days.
- Require every line in one order to use the same currency.
- Change quantities and remove lines while an order is pending.
- Confirm only a non-empty order.
- Cancel a pending or confirmed order.
- Move an order through `Pending -> Confirmed -> Delivered -> Completed`.
- Lock line-item changes after the order leaves `Pending`.

## Domain Topology

The model uses identifier references instead of public navigation properties:

```text
Category.Id <----- Product.CategoryId (optional)

Role.Id <--------- User.RolesId (zero or more)

User.Id <--------- Order.UserId
                     |
                     +--- owns OrderItem collection
                              |
Product.Id <------------------+--- OrderItem.ProductId
```

The `Order`/`OrderItem` relationship is the strongest ownership boundary in the current model:

- `Order` exposes items as a read-only collection.
- `OrderItem` creation methods are `internal`.
- Quantity changes are initiated through `Order.ChangeItemQuantity`.
- Order totals are recalculated by `Order`, not supplied by callers.

This makes `Order` the effective aggregate root and `OrderItem` an aggregate child.

## Shared Technical Foundations

### `BaseEntity`

Every entity inherits [`BaseEntity`](../../Domain/Abstractions/BaseEntity.cs) and receives:

| Property | Behavior |
|---|---|
| `Id` | A new `Guid` is generated when the object is constructed. |
| `CreatedAt` | Set to `DateTime.UtcNow` at construction time. |
| `IsDeleted` | Initialized to `false`; setters are protected. |

The base type supplies soft-delete state, but the domain currently provides no public delete or restore behavior. Persistence or a future domain method must control changes to `IsDeleted`.

### Value objects

Primitive business values are represented by dedicated types:

- `CategoryName`
- `ProductName`
- `ProductDescription`
- `RoleName`
- `FullName`
- `Email`
- `PasswordHash`
- `PhoneNumber`
- `DateRange`
- `Money`

Most are immutable records, so their validated value cannot be modified after construction. `Email` is currently a class rather than a record; its implementation caveat is documented in the [User guide](User.md).

### Money and currency

[`Money`](../../Domain/Shared/ValueObjects/Money.cs) combines a decimal `Amount` with a `Currency`. Supported currencies are currently:

- `EUR`
- `USD`

Money addition rejects different currencies. Multiplication accepts only a positive integer. A standalone `Money` can contain zero, but an `OrderItem` requires a strictly positive unit price.

### Encapsulation

The entities generally use:

- Private constructors and named static factories for controlled creation.
- Private property setters to stop arbitrary external state mutation.
- Behavior methods such as `EnableForRental`, `AddRole`, or `Confirm` instead of public setters.
- Domain-specific exceptions for invalid business operations.

### Exception strategy

Invalid operations fail immediately by throwing an exception. The model uses:

- Standard exceptions for programmer-level argument violations, such as an empty user or product ID.
- `ArgumentNullException` for required object arguments.
- Domain-specific exceptions for business rules, such as invalid order transitions, invalid names, mixed currencies, or missing order items.

An application layer integrating this model should translate these exceptions into appropriate API validation responses instead of exposing raw exception details.

## Cross-Entity Rules

| Rule | Enforced by |
|---|---|
| Product category ID cannot be empty when assigned | `Product.AssignCategory` |
| User role ID cannot be empty or duplicated | `User.AddRole` |
| Order user ID cannot be empty | `Order.Create` |
| Order-item product and order IDs cannot be empty | `OrderItem` creation |
| Order quantities must be greater than zero | `OrderItem` |
| Order-item prices must be greater than zero | `OrderItem` |
| Rental item must have a date range | `OrderItem` |
| Sale item must not have a date range | `OrderItem` |
| Rental start cannot be in the past | `Order.AddRentalItem` |
| All order lines must have the same currency | `Order.AddItem` |
| Items are mutable only while order is pending | `Order` |
| Empty order cannot be confirmed | `Order.Confirm` |
| Status changes must follow the defined lifecycle | `Order` |

## Explicit Model Boundaries

The following concerns are not enforced by the current `Domain` project:

- Product stock reservation, decrement, release, and overselling protection.
- Verifying that referenced category, role, user, or product IDs exist.
- Unique category names, product names, role names, or email addresses.
- Product URL format and media availability.
- Password hashing or password-strength policy; the domain accepts an existing hash.
- Payment authorization, capture, refund, taxes, discounts, shipping, and invoices.
- Rental availability overlap, pickup, return, overdue fees, or damage handling.
- Domain events, audit history beyond creation time, and concurrency/version tokens.
- Repository interfaces, units of work, or persistence mappings for the new entities.

These are application, infrastructure, or future domain responsibilities and must not be assumed from the current model.

## Recommended Use of This Documentation

- Product owners can use each guide's business sections to verify supported workflows.
- API developers can use method and exception tables when building commands and error mappings.
- Persistence developers can use the ownership and value-object sections when creating EF Core configurations.
- Testers can use the scenario lists as acceptance and unit-test candidates.
- Maintainers should update the relevant entity guide whenever a domain invariant or behavior changes.

## Source Reference

The documented project targets `.NET 10`, enables nullable reference types, and uses implicit global usings. Its project definition is available at [`Domain/Domain.csproj`](../../Domain/Domain.csproj).