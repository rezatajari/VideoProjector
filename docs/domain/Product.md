# Product Entity Guide

## Overview

`Product` represents one catalog item and the commercial terms under which that item may be sold, rented, or offered through both channels.

The entity owns:

- A generated `Guid` identity.
- A validated product name.
- An optional product description.
- An optional category reference.
- Independent sale price and sale quantity state.
- Independent daily-rental price and rental quantity state.
- An image URL and an optional test-video URL.
- Creation and soft-delete metadata inherited from `BaseEntity`.

The model protects several local state transitions through private setters and behavior methods. It does not reserve inventory, decrement stock, validate referenced records, or make a product orderable by itself.

This guide describes the newer rich domain model in `Domain/Products`. The running API and client currently use the separate mutable `Shared.Models.Product` model, so the domain rules described here are not automatically enforced at runtime.

## Source Files

The rich product model is implemented by:

- [`Domain/Products/Product.cs`](../../Domain/Products/Product.cs)
- [`Domain/Products/ValueObjects/ProductName.cs`](../../Domain/Products/ValueObjects/ProductName.cs)
- [`Domain/Products/ValueObjects/ProductDescription.cs`](../../Domain/Products/ValueObjects/ProductDescription.cs)
- [`Domain/Products/Exceptions/InvalidProductNameException.cs`](../../Domain/Products/Exceptions/InvalidProductNameException.cs)
- [`Domain/Products/Exceptions/InvalidProductDescriptionException.cs`](../../Domain/Products/Exceptions/InvalidProductDescriptionException.cs)
- [`Domain/Products/Exceptions/InvalidQuantityException.cs`](../../Domain/Products/Exceptions/InvalidQuantityException.cs)
- [`Domain/Categories/Exceptions/InvalidCategoryIdException.cs`](../../Domain/Categories/Exceptions/InvalidCategoryIdException.cs)
- [`Domain/Shared/ValueObjects/Money.cs`](../../Domain/Shared/ValueObjects/Money.cs)
- [`Domain/Shared/ValueObjects/Currency.cs`](../../Domain/Shared/ValueObjects/Currency.cs)
- [`Domain/Abstractions/BaseEntity.cs`](../../Domain/Abstractions/BaseEntity.cs)

Related ordering behavior is implemented by:

- [`Domain/Orders/Order.cs`](../../Domain/Orders/Order.cs)
- [`Domain/Orders/OrderItem.cs`](../../Domain/Orders/OrderItem.cs)

The currently running application uses these separate legacy files:

- [`Shared/Models/Product.cs`](../../Shared/Models/Product.cs)
- [`Shared/Models/Order.cs`](../../Shared/Models/Order.cs)
- [`API/Controllers/ProductsController.cs`](../../API/Controllers/ProductsController.cs)
- [`API/Controllers/OrdersController.cs`](../../API/Controllers/OrdersController.cs)
- [`API/Data/Configurations/ProductConfiguration.cs`](../../API/Data/Configurations/ProductConfiguration.cs)
- [`Client/Services/ProductService.cs`](../../Client/Services/ProductService.cs)
- [`Client/Pages/ManageProduct.razor`](../../Client/Pages/ManageProduct.razor)
- [`Client/Pages/ProductsCatalog.razor`](../../Client/Pages/ProductsCatalog.razor)

## Business Purpose

A product is the catalog-side source of descriptive data, commercial prices, and stock quantities. Sale and rental are modeled as separate offerings because their price units and available quantities differ:

- A sale offering has a one-time unit price and a sale quantity.
- A rental offering has a daily unit price and a rental quantity.

One product may carry data for either offering or for both offerings at the same time. The current implementation does not require sale and rental inventory to represent physically separate units, and it does not coordinate stock across those channels.

## Business Capabilities

The current product model can:

- Create a product from typed name and description values.
- Store an image URL and optional test-video URL at creation time.
- Assign a non-empty category ID.
- Remove an existing category reference.
- Enable a sale offering with a `Money` price and positive quantity.
- Disable a sale offering by clearing its price and quantity.
- Enable a rental offering with a daily `Money` price and positive quantity.
- Disable a rental offering by clearing its price and quantity.
- Retain independent sale and rental price/quantity pairs.

The current product model cannot:

- Rename a product.
- Change its description or media URLs after creation.
- Verify that an assigned category exists or is active.
- Reserve, decrement, replenish, or release inventory.
- Prevent overselling or overlapping rental reservations.
- Validate image or video URL syntax.
- Enforce unique product names or stock-keeping units.
- Apply discounts, taxes, deposits, or time-based pricing.
- Record product variants, serial numbers, or physical inventory units.
- Soft-delete or restore itself through a public method.

## Position in the Domain Model

`Product` is referenced by identifier rather than through navigation properties:

```text
Category.Id <----- Product.CategoryId (optional)

Product.Id <------ OrderItem.ProductId
                         |
                         +--- owned by Order
```

These references do not make `Product` part of the `Order` aggregate:

- `Product` does not contain orders or order items.
- `OrderItem` stores only a `ProductId`, not a `Product` object.
- `Order.AddSaleItem` and `Order.AddRentalItem` receive prices from the caller.
- Creating an order item does not read or mutate product inventory.
- Changing a product price later does not recalculate an existing order item.

`Product` and `Order` are therefore separate consistency boundaries. An application service must load both, validate availability, snapshot the correct price into the order, and coordinate inventory changes transactionally.

## State

| Property | Type | Business meaning | How it changes |
|---|---|---|---|
| `Id` | `Guid` | Stable product identity | Generated by `BaseEntity` |
| `CategoryId` | `Guid?` | Optional category reference | `AssignCategory` or `RemoveCategory` |
| `ProductName` | `ProductName` | Validated display/catalog name | Set during construction only |
| `ProductDescription` | `ProductDescription?` | Optional normalized description | Set during construction only |
| `SalePrice` | `Money?` | One-time price for one sold unit | `EnableForSale` or `DisableForSale` |
| `QuantityForSale` | `int` | Quantity assigned to the sale channel | `EnableForSale` or `DisableForSale` |
| `RentalPricePerDay` | `Money?` | Price for one rented unit per day | `EnableForRental` or `DisableForRental` |
| `QuantityForRental` | `int` | Quantity assigned to the rental channel | `EnableForRental` or `DisableForRental` |
| `IsRental` | `bool` | Last enable-operation mode marker | Set by enable methods only |
| `ImageUrl` | `string` | Product image location | Supplied during construction only |
| `TestVideoUrl` | `string?` | Optional product test-video location | Supplied during construction only |
| `CreatedAt` | `DateTime` | UTC entity creation timestamp | Generated by `BaseEntity` |
| `IsDeleted` | `bool` | Inherited soft-delete state | Initialized to `false`; no product behavior changes it |

New products begin with:

- No category.
- No sale price and a sale quantity of zero.
- No rental price and a rental quantity of zero.
- `IsRental == false`.
- `IsDeleted == false`.

## Creation Model

The constructor is private. Normal callers use the static factory:

```csharp
public static Product Create(
    ProductName productName,
    ProductDescription? productDescription,
    string imageUrl,
    string? testVideoUrl)
```

Example:

```csharp
var product = Product.Create(
    new ProductName("  Epson EB-X41  "),
    new ProductDescription("  3LCD business projector  "),
    "/images/epson-eb-x41.png",
    "https://media.example.test/epson-eb-x41.mp4");
```

The resulting state includes:

| State | Result |
|---|---|
| `ProductName.Value` | `"Epson EB-X41"` |
| `ProductDescription.Value` | `"3LCD business projector"` |
| `ImageUrl` | `"/images/epson-eb-x41.png"` |
| `TestVideoUrl` | Supplied URL |
| `CategoryId` | `null` |
| Sale offering | Disabled |
| Rental offering | Disabled |
| `Id` | Newly generated `Guid` |
| `CreatedAt` | Current UTC timestamp |

### Factory validation boundary

`Product.Create` performs no validation itself. It relies on the supplied value objects and stores all four arguments directly.

Consequences of the current implementation include:

- `Product.Create(null!, description, imageUrl, testVideoUrl)` succeeds at runtime and produces a null `ProductName` reference.
- `imageUrl` is not checked for null, emptiness, whitespace, URL syntax, or reachability.
- Passing `null!` as `imageUrl` overwrites the property initializer and produces a null runtime value despite the non-nullable declaration.
- `testVideoUrl` accepts any string, including empty or malformed text.
- The factory does not require at least one sale or rental offering.

The non-nullable parameter annotations provide compiler guidance but are not runtime guards.

### Image default caveat

`ImageUrl` has this property initializer:

```csharp
public string ImageUrl { get; private set; } = "/images/default-projector.png";
```

The private constructor always replaces that value with its `imageUrl` argument. There is no factory overload that omits the image URL, so normal factory creation uses the caller-supplied value rather than automatically retaining the default.

Callers that want the documented default must currently pass `"/images/default-projector.png"` explicitly. The initializer may still matter to some materialization paths, but it is not a fallback for blank or null factory input.

## Product Name Rules

`ProductName` is an immutable sealed record. It validates and normalizes the name before exposing it through `Value`.

| Input | Behavior |
|---|---|
| `null`, empty, or whitespace-only | Rejected |
| Leading or trailing whitespace | Removed with `Trim()` |
| 1 to 100 characters after trimming | Accepted |
| More than 100 characters after trimming | Rejected |
| Internal whitespace | Preserved |
| Letter casing | Preserved |
| Character alphabet | Not restricted |

Example:

```csharp
var name = new ProductName("  Epson EB-X41  ");

// name.Value == "Epson EB-X41"
```

The type does not:

- Collapse repeated internal spaces.
- Convert casing.
- Enforce uniqueness.
- Validate a manufacturer or model-number format.
- Generate a slug or SKU.

Because `ProductName` is a record, equality is based on normalized value:

```csharp
var first = new ProductName("  Epson EB-X41  ");
var second = new ProductName("Epson EB-X41");

// first == second
```

String equality remains case-sensitive, so `"Epson EB-X41"` and `"epson eb-x41"` are different values.

## Product Description Rules

`ProductDescription` is an immutable sealed record whose `Value` is nullable.

| Input | Behavior |
|---|---|
| `null`, empty, or whitespace-only | Accepted; `Value` remains `null` |
| Leading or trailing whitespace | Removed with `Trim()` |
| 1 to 255 characters after trimming | Accepted |
| More than 255 characters after trimming | Rejected |
| Internal whitespace and casing | Preserved |

Example:

```csharp
var description = new ProductDescription("  HDMI and VGA inputs  ");

// description.Value == "HDMI and VGA inputs"
```

There are two representable forms of an absent description on `Product`:

```csharp
ProductDescription? absentObject = null;
var emptyValueObject = new ProductDescription("   ");

// absentObject is null
// emptyValueObject is not null
// emptyValueObject.Value is null
```

These forms are conceptually similar but structurally different. Persistence and serialization code should choose one canonical representation to avoid inconsistent null handling.

## Category Assignment

Category membership is optional and represented by a `Guid?`:

```csharp
product.AssignCategory(category.Id);
product.RemoveCategory();
```

`AssignCategory` rejects only `Guid.Empty`. It does not verify:

- That the category exists.
- That the category is not soft-deleted.
- That the category is suitable for this product.
- That the assignment differs from the current value.

Assigning the same non-empty ID again succeeds and leaves the same state. Assigning a different non-empty ID replaces the previous reference. `RemoveCategory` is idempotent and succeeds even when `CategoryId` is already null.

The application layer should resolve the category from trusted persistence before assignment when referential validity matters.

## Sale Offering

A sale offering is enabled with:

```csharp
product.EnableForSale(
    new Money(1_250m, Currency.USD),
    quantity: 3);
```

The method executes these rules in order:

1. Reject `quantity <= 0`.
2. Reject a null `salePrice`.
3. Store the supplied `Money` value.
4. Store the positive quantity.
5. Set `IsRental` to `false`.

It does not change `RentalPricePerDay` or `QuantityForRental`.

The offering is disabled with:

```csharp
product.DisableForSale();
```

This operation:

- Sets `SalePrice` to `null`.
- Sets `QuantityForSale` to `0`.
- Does not modify rental state.
- Does not modify `IsRental`.

Calling `EnableForSale` again replaces the complete sale price/quantity pair. There is no separate public method for changing only the sale price, adding stock, or removing a specific number of units.

## Rental Offering

A rental offering is enabled with:

```csharp
product.EnableForRental(
    new Money(75m, Currency.USD),
    quantity: 2);
```

The method executes these rules in order:

1. Reject `quantity <= 0`.
2. Reject a null `rentalPricePerDay`.
3. Store the supplied daily `Money` value.
4. Store the positive rental quantity.
5. Set `IsRental` to `true`.

It does not change `SalePrice` or `QuantityForSale`.

The offering is disabled with:

```csharp
product.DisableForRental();
```

This operation:

- Sets `RentalPricePerDay` to `null`.
- Sets `QuantityForRental` to `0`.
- Does not modify sale state.
- Does not reset `IsRental`.

Calling `EnableForRental` again replaces the complete rental price/quantity pair. The domain does not model reservation calendars, overlapping rental periods, pickup, return, or per-unit rental state.

## Independent Offering State

The sale and rental methods do not clear one another. A product can therefore hold both offerings simultaneously:

```csharp
product.EnableForSale(
    new Money(1_250m, Currency.USD),
    quantity: 3);

product.EnableForRental(
    new Money(75m, Currency.USD),
    quantity: 2);

// SalePrice is not null
// QuantityForSale == 3
// RentalPricePerDay is not null
// QuantityForRental == 2
```

For state produced only through the intended behavior methods, offering availability is represented by each price/quantity pair:

| Commercial state | Sale pair | Rental pair |
|---|---|---|
| Neither offering enabled | `null`, `0` | `null`, `0` |
| Sale only | Non-null, positive | `null`, `0` |
| Rental only | `null`, `0` | Non-null, positive |
| Both | Non-null, positive | Non-null, positive |

The model does not require at least one enabled offering. A catalog-only or temporarily unavailable product is valid.

## `IsRental` Semantics and Caveat

`IsRental` is not a reliable indicator of current rental availability or exclusive product type. It records only which enable method most recently assigned it:

- A new product starts with `false`.
- `EnableForSale` sets it to `false`.
- `EnableForRental` sets it to `true`.
- Neither disable method changes it.

Examples:

```csharp
product.EnableForRental(rentalPrice, 2);
product.EnableForSale(salePrice, 3);

// Both offerings remain enabled.
// IsRental == false because sale was enabled last.
```

```csharp
product.EnableForRental(rentalPrice, 2);
product.DisableForRental();

// RentalPricePerDay is null.
// QuantityForRental == 0.
// IsRental is still true.
```

Code must not use `IsRental` to answer any of these questions:

- Is this product currently rentable?
- Is this product rental-only?
- Which offering pairs are configured?
- Should the rental purchase action be displayed?

Until the model is revised, the price and quantity pairs are the authoritative current state. A future design should either remove `IsRental`, replace it with derived properties such as `IsAvailableForSale` and `IsAvailableForRental`, or define a deliberate offering-mode enum with complete transition rules.

## Money and Currency Rules

Both prices use the shared `Money` value object:

```csharp
new Money(decimal amount, Currency currency)
```

`Money` enforces:

- Amounts below zero are rejected.
- Zero is accepted.
- Currency is stored with the amount.
- Addition requires matching currencies.
- Multiplication requires a positive integer.

Supported named currency values are currently:

- `EUR`
- `USD`

The product does not require its sale and rental prices to use the same currency:

```csharp
product.EnableForSale(new Money(1_250m, Currency.USD), 3);
product.EnableForRental(new Money(70m, Currency.EUR), 2);
```

That state is accepted. Whether mixed offering currencies are valid is an application policy not currently encoded in `Product`.

### Zero-price mismatch

`Money` and the product enable methods allow a zero price:

```csharp
product.EnableForSale(new Money(0m, Currency.USD), 1);
```

The rich `OrderItem` model requires its unit price to be strictly greater than zero. Passing this enabled product price into `Order.AddSaleItem` therefore fails with `InvalidOrderPriceException`.

The product and ordering models currently disagree about whether a free offering is valid. An integration must resolve this rule explicitly rather than assuming every enabled product price can create an order item.

### Undefined enum values

`Money` stores the supplied `Currency` enum without checking `Enum.IsDefined`. A cast such as `(Currency)999` can therefore enter the model. Request validation or a future value-object guard should reject undefined values at system boundaries.

## Media Fields

`ImageUrl` and `TestVideoUrl` are plain strings rather than value objects.

The domain does not validate:

- Absolute versus relative paths.
- URI schemes.
- Host allowlists.
- File type or extension.
- URL length.
- Resource existence or accessibility.
- Whether a test-video URL is safe to embed or open.

Media upload, storage ownership, content scanning, URL normalization, and outbound-link security belong to application and infrastructure layers unless dedicated domain rules are introduced.

## Ordering Boundary

The rich order model accepts product information as arguments:

```csharp
order.AddSaleItem(
    product.Id,
    quantity: 1,
    unitPrice: product.SalePrice!);
```

```csharp
order.AddRentalItem(
    product.Id,
    duration,
    quantity: 1,
    unitPricePerDay: product.RentalPricePerDay!,
    today);
```

Neither method receives a `Product` entity. As a result, `Order` and `OrderItem` do not verify:

- That the product exists.
- That it is not soft-deleted.
- That the selected offering is enabled.
- That requested quantity is in stock.
- That the supplied price matches the product price.
- That the supplied currency matches current catalog policy.
- That a rental period is available for the requested units.

The application layer must perform those checks using trusted, freshly loaded state. Client-supplied prices and availability flags must not be treated as authoritative.

### Price snapshot

Once added, an `OrderItem` stores its own `UnitPrice` and calculated `LineTotal`. A later product price change does not affect the line. This is appropriate for a price snapshot, but the application must ensure that the snapshot came from trusted product state at the intended point in the workflow.

### Inventory consistency

The rich `Product` entity has no methods such as `ReserveForSale`, `ReserveForRental`, `ReleaseStock`, or `CompleteReservation`. Enabling an offering assigns a complete quantity; ordering does not consume it.

A production workflow needs a defined inventory policy and a transactional or concurrency-controlled implementation. A read-then-check-then-write sequence without a lock, version token, or atomic database condition can oversell under concurrent requests.

## Domain Invariants

For a product created and modified only through the intended public API with valid non-null factory arguments:

| Invariant | Enforced by |
|---|---|
| Product ID is generated | `BaseEntity` |
| Creation timestamp is generated in UTC | `BaseEntity` |
| Soft-delete state starts as `false` | `BaseEntity` |
| Product name is not blank | `ProductName` |
| Product name is at most 100 normalized characters | `ProductName` |
| Description is absent or at most 255 normalized characters | `ProductDescription` |
| Assigned category ID is non-empty | `AssignCategory` |
| Enabled sale quantity is positive | `EnableForSale` |
| Enabled sale price reference is non-null | `EnableForSale` |
| Disabled sale state has null price and zero quantity | `DisableForSale` |
| Enabled rental quantity is positive | `EnableForRental` |
| Enabled rental price reference is non-null | `EnableForRental` |
| Disabled rental state has null price and zero quantity | `DisableForRental` |
| Money amount is non-negative | `Money` |
| Public callers cannot set state properties directly | Private setters |

The following potential rules are not enforced:

| Potential rule | Current behavior |
|---|---|
| Factory name argument must be non-null | `Product.Create(null!, ...)` succeeds |
| Image URL must be non-null or nonblank | No runtime guard exists |
| Media values must be valid URLs | No URL parsing occurs |
| Product must have a category | `CategoryId` starts and may remain null |
| Assigned category must exist | Only non-empty ID is checked |
| At least one offering must be enabled | A product with neither offering is valid |
| Enabled prices must be greater than zero | Zero-valued `Money` is accepted |
| Sale and rental currencies must match | Each offering accepts its own currency |
| `IsRental` must match rental availability | Disable operations do not maintain it |
| Product names must be unique | No repository lookup occurs |
| Requested inventory must be available | No order or reservation input is accepted by `Product` |
| Inventory cannot be oversold | No decrement or concurrency behavior exists |
| Product data can be edited | Name, description, and media have no update methods |

## Failure Reference

| Operation | Invalid condition | Exception | Current message |
|---|---|---|---|
| `new ProductName(value)` | Null, empty, or whitespace-only | `InvalidProductNameException` | `Product name cannot be null or whitespace.` |
| `new ProductName(value)` | Normalized length exceeds 100 | `InvalidProductNameException` | `Product name cannot be longer than 100 characters.` |
| `new ProductDescription(value)` | Normalized length exceeds 255 | `InvalidProductDescriptionException` | `Value is too long. Maximum length is 255` |
| `product.AssignCategory(id)` | `id == Guid.Empty` | `InvalidCategoryIdException` | `Category id  cannot be empty` |
| `product.EnableForSale(price, quantity)` | `quantity <= 0` | `InvalidQuantityException` | `Quantity cannot be negative.` |
| `product.EnableForSale(null!, positiveQuantity)` | Null price | `ArgumentNullException` | Standard framework message for `salePrice` |
| `product.EnableForRental(price, quantity)` | `quantity <= 0` | `InvalidQuantityException` | `Quantity cannot be negative.` |
| `product.EnableForRental(null!, positiveQuantity)` | Null price | `ArgumentNullException` | Standard framework message for `rentalPricePerDay` |
| `new Money(amount, currency)` | `amount < 0` | `InvalidMoneyAmountException` | `Amount must be greater than zero` |

Some messages are less precise than the implemented condition:

- Quantity zero is rejected even though the message mentions only negative quantity.
- Money zero is accepted even though the message says the amount must be greater than zero.
- The category message contains two spaces between `id` and `cannot`.

Consumers should map exception types and operation context to stable validation responses rather than parsing these message strings.

### Validation order

Both enable methods validate quantity before checking price for null:

```csharp
product.EnableForSale(null!, 0);
```

This call throws `InvalidQuantityException`, not `ArgumentNullException`, because execution stops at the quantity guard.

## Encapsulation and Mutability

| Member | Accessibility | Effect |
|---|---|---|
| `Product` constructor | `private` | Directs normal creation through `Create` |
| `Product.Create` | `public static` | Creates initial descriptive/media state |
| State property setters | `private` | Prevent arbitrary external mutation |
| `AssignCategory` | `public` | Sets a validated non-empty category ID |
| `RemoveCategory` | `public` | Clears the category ID |
| `EnableForSale` | `public` | Replaces sale price and quantity |
| `DisableForSale` | `public` | Clears sale price and quantity |
| `EnableForRental` | `public` | Replaces rental price and quantity |
| `DisableForRental` | `public` | Clears rental price and quantity |

`Product` is sealed, so behavior cannot be changed by deriving a subtype.

The entity is mutable only in its category and commercial offering state. Its name, description, image URL, and test-video URL cannot be changed through the public domain API after construction.

## Entity and Value Equality

`ProductName`, `ProductDescription`, and `Money` are records, so each has structural value equality.

`Product` is a class and does not override equality. Two separate products with equal descriptive and offering values are still separate object references and normally receive different IDs:

```csharp
var first = Product.Create(name, description, imageUrl, null);
var second = Product.Create(name, description, imageUrl, null);

// first.Id != second.Id under normal construction
// first != second by reference equality
```

Code comparing persisted product identity should compare `Id` explicitly unless a common entity-equality strategy is added later.

## Persistence Considerations for the Rich Model

The `Domain.Products.Product` entity has no EF Core configuration or migration in the current repository. A future mapping must deliberately handle its value objects and invariants.

Suggested mapping concerns include:

| Domain state | Persistence concern |
|---|---|
| `Id` | `Guid` primary key |
| `ProductName.Value` | Required string, maximum length 100 |
| `ProductDescription.Value` | Nullable string, maximum length 255 |
| `CategoryId` | Nullable foreign key to category |
| `SalePrice` | Nullable amount and currency columns or owned/complex value |
| `QuantityForSale` | Required integer with non-negative constraint |
| `RentalPricePerDay` | Nullable amount and currency columns or owned/complex value |
| `QuantityForRental` | Required integer with non-negative constraint |
| `IsRental` | Requires redesign or clearly documented legacy semantics |
| `ImageUrl` | Decide required/default behavior and maximum length |
| `TestVideoUrl` | Nullable string with an intentional maximum length |
| `CreatedAt` | UTC timestamp policy |
| `IsDeleted` | Soft-delete query policy |

Database constraints should preserve the intended price/quantity pair states. Conceptually:

```text
(SalePrice is null AND QuantityForSale = 0)
OR
(SalePrice is not null AND QuantityForSale > 0)
```

and the same rule for rental state.

Additional persistence decisions are required for:

- Currency storage as names or numeric enum values.
- Rejecting undefined currencies.
- Category delete behavior.
- Canonical representation of an absent description.
- Optimistic concurrency tokens for stock-sensitive changes.
- Whether soft-deleted products may remain referenced by historical order items.
- Whether product names need a unique normalized index.

Persistence materialization must not create states that public behavior methods would reject.

## Runtime Integration Status

The running API does not currently use `Domain.Products.Product`.

`VideoProjectorDbContext` exposes `DbSet<Shared.Models.Product>`, and `ProductConfiguration` also imports `Shared.Models`. `API/API.csproj` references `Shared`, not the rich `Domain` project. The client serializes the same legacy model directly.

Therefore:

- Rich `ProductName`, `ProductDescription`, and `Money` validation is not applied to API requests.
- Rich category-ID behavior is not used.
- Rich `EnableForSale` and `EnableForRental` transitions are not used.
- Rich `Guid` identity is not used by product endpoints.
- Existing EF migrations describe the legacy model, not this entity.

The two models must not be treated as interchangeable.

## Rich Model and Legacy Model Comparison

| Concern | Rich `Domain.Products.Product` | Active `Shared.Models.Product` |
|---|---|---|
| Identity | `Guid` | `int` |
| Name | `ProductName`, max 100 | Mutable `string`; EF max 150 |
| Description | `ProductDescription?`, max 255 | Mutable `string?`; EF max 1000 |
| Category | Optional `Guid? CategoryId` | Required mutable category string |
| Sale price | `Money?` with currency | `decimal?` |
| Rental price | `Money?` with currency | `decimal?` |
| Sale quantity | Private setter, changed through methods | Public mutable `int` |
| Rental quantity | Private setter, changed through methods | Public mutable `int` |
| Offering methods | Explicit enable/disable methods | Direct property assignment |
| Rental marker | Ambiguous `IsRental` flag | No product-level flag |
| Media | Constructor-only strings | Public mutable strings |
| Runtime mapping | None | EF Core configuration and migrations |
| API contract | Not exposed | Entity is accepted and returned directly |

The legacy client displays monetary values as `toman`, while the rich `Currency` enum supports only `EUR` and `USD`. Currency migration is therefore a business-data decision, not just a mechanical type conversion.

## Active API Behavior

The current `ProductsController` exposes legacy CRUD endpoints:

| Endpoint | Authorization | Current behavior |
|---|---|---|
| `GET /api/products` | Authenticated user | Returns products where `IsDeleted == false` |
| `GET /api/products/{id}` | Authenticated user | Returns one non-deleted product or `404` |
| `POST /api/products` | `Admin` role | Persists the request-bound legacy entity |
| `PUT /api/products/{id}` | `Admin` role | Replaces tracked state from a request-bound legacy entity after ID/existence checks |
| `DELETE /api/products/{id}` | `Admin` role | Sets legacy `IsDeleted` to `true` |

The API accepts persistence entities directly rather than command DTOs. The legacy product has no data-annotation validation, and its EF configuration mainly supplies lengths, required columns, decimal types, and defaults. It does not encode rich domain transitions or database checks for valid price/quantity combinations.

Notable runtime differences include:

- Negative or inconsistent quantities are not rejected by the product controller.
- Negative or zero prices are not rejected by the product controller.
- A nullable price can be combined with a positive quantity.
- A non-null price can be combined with zero or negative quantity.
- Category is a free request-bound string at the model level.
- Full-entity update can bind metadata such as `CreatedAt` and `IsDeleted`.

These are characteristics of the current runtime path, not capabilities of the rich domain entity.

## Active Ordering and Inventory Behavior

The legacy `OrdersController` currently coordinates product stock directly.

### Sale orders

For a non-rental order, it:

1. Loads a non-deleted legacy product.
2. Rejects a null sale price.
3. Rejects requested quantity greater than sale stock.
4. Subtracts order quantity from `QuantityForSale`.
5. Calculates total from the legacy decimal price.

The endpoint does not reject zero or negative order quantity. A negative quantity can pass the stock comparison, increase sale stock during subtraction, and produce a negative total. Request validation is required before this workflow can be considered safe.

Canceled sale orders restore sale quantity when the status changes to `Canceled` from a different status. Status transitions are not constrained by the rich order lifecycle, so repeated transitions can undermine inventory accounting.

### Rental orders

For a rental order, the controller validates dates and compares requested quantity with `QuantityForRental`, but it does not decrement or reserve rental quantity. It also calculates with:

```csharp
product.RentalPricePerDay ?? 0
```

A direct API request can therefore create a zero-priced rental when rental stock is positive but rental price is null. The client normally hides the rental action in that state, but client visibility is not an authorization or validation boundary.

### Concurrency

The legacy model has no concurrency token and the workflow uses application-side read/check/update logic. Concurrent sale requests can both observe sufficient stock before saving. Robust inventory enforcement needs an atomic database condition, appropriate locking, or optimistic concurrency with retry/rejection behavior.

## Active Client Behavior

The current Blazor client uses `Shared.Models.Product` through `ProductService`.

The admin form:

- Allows direct editing of name, description, category string, both prices, and both quantities.
- Uses three fixed Persian category values in the UI.
- Does not invoke rich product methods or value objects.
- Sends the complete mutable model to `POST` or `PUT` endpoints.

The catalog:

- Shows rental price whenever `RentalPricePerDay` has a value.
- Shows sale price whenever `SalePrice` has a value.
- Shows the rental action only when rental price is non-null and rental quantity is positive.
- Shows the sale action only when sale price is non-null and sale quantity is positive.
- Uses a client-side fallback image path when the URL is blank.
- Opens `TestVideoUrl` in a new browser tab when it is non-empty.

These display checks improve the normal user flow but do not replace server-side validation.

## Integration Guidance

A safe migration to the rich product model should be treated as an application and persistence change, not as a namespace substitution.

A practical sequence is:

1. Decide canonical currency behavior, including how existing toman values map to the rich model.
2. Resolve category migration from free strings to category IDs.
3. Decide whether zero prices are allowed and align `Money`, `Product`, and `OrderItem`.
4. Replace or redefine `IsRental` so offering availability is unambiguous.
5. Define inventory reservation, release, sale completion, and rental-period policies.
6. Add EF Core mappings and constraints for rich entities and value objects.
7. Introduce request/response DTOs instead of binding domain or persistence entities directly.
8. Implement application commands that load trusted product state and call domain behavior.
9. Add concurrency handling for all stock-sensitive operations.
10. Migrate existing data and update client contracts from `int` IDs and decimals to the chosen rich representations.
11. Translate domain exceptions into stable API validation responses.
12. Add unit, integration, authorization, and concurrency tests before switching runtime traffic.

During migration, avoid maintaining two writable product sources without an explicit synchronization and ownership strategy.

## Security and Trust Boundaries

Product management and ordering code should treat request data as untrusted.

At minimum, an application layer should:

- Resolve product and category IDs from persistence.
- Reject missing, deleted, or unavailable products.
- Take prices from server-side product state, not request payloads.
- Validate positive order quantity.
- Validate rental dates against an injected clock or supplied business date.
- Recheck inventory in the same consistency boundary as reservation or decrement.
- Authorize catalog management independently from catalog viewing.
- Avoid allowing request DTOs to set identity, creation time, or soft-delete state.
- Validate and normalize media inputs according to storage and browser-security policy.
- Audit price, stock, category, and availability changes where required.

## Test Scenarios

### Creation and value objects

1. Creating a product name trims surrounding whitespace.
2. Blank product names fail with `InvalidProductNameException`.
3. Names of exactly 100 normalized characters succeed.
4. Names longer than 100 normalized characters fail.
5. Descriptions trim surrounding whitespace.
6. Blank descriptions produce a value object whose `Value` is null.
7. Descriptions of exactly 255 normalized characters succeed.
8. Descriptions longer than 255 normalized characters fail.
9. Product creation initializes both offerings as disabled.
10. Product creation preserves supplied media strings without normalization.
11. A test documents the current null-name factory gap until it is fixed.
12. A test documents that blank image input is currently accepted.

### Category behavior

1. Assigning a non-empty category ID stores it.
2. Assigning `Guid.Empty` fails with `InvalidCategoryIdException`.
3. Reassignment replaces the existing category ID.
4. Removing a category clears the ID.
5. Removing an already absent category is idempotent.

### Sale behavior

1. Enabling sale stores the exact `Money` value and positive quantity.
2. Enabling sale with zero quantity fails.
3. Enabling sale with negative quantity fails.
4. Enabling sale with null price and positive quantity fails.
5. Re-enabling sale replaces its prior price and quantity.
6. Disabling sale clears price and resets quantity to zero.
7. Disabling sale does not change rental state.
8. Zero-valued sale money is currently accepted.

### Rental behavior

1. Enabling rental stores the exact daily `Money` value and positive quantity.
2. Enabling rental with zero quantity fails.
3. Enabling rental with negative quantity fails.
4. Enabling rental with null price and positive quantity fails.
5. Re-enabling rental replaces its prior price and quantity.
6. Disabling rental clears price and resets quantity to zero.
7. Disabling rental does not change sale state.
8. Zero-valued rental money is currently accepted.

### Combined offering and marker behavior

1. Enabling sale then rental leaves both offerings configured.
2. Enabling rental then sale leaves both offerings configured.
3. `IsRental` is true after rental is enabled last.
4. `IsRental` is false after sale is enabled last.
5. Disabling rental after enabling it leaves the current stale `IsRental == true` behavior documented.
6. Disabling either channel does not mutate the other channel.
7. Different sale and rental currencies are currently accepted.

### Ordering and integration

1. An application command rejects a nonexistent or deleted product.
2. A sale command rejects a disabled sale offering.
3. A rental command rejects a disabled rental offering.
4. Server-side product price is used instead of a client-supplied price.
5. Requested quantity must be positive and within available inventory.
6. Existing order-item price remains unchanged after a product price update.
7. A zero product price is handled consistently with the chosen order rule.
8. Concurrent requests cannot consume more sale inventory than exists.
9. Overlapping rental reservations cannot exceed available units.
10. Canceling an order releases inventory exactly once according to policy.

## Known Gaps and Design Questions

The current model leaves these decisions open:

- Should a product be allowed with neither sale nor rental offering?
- Are sale and rental stocks separate pools or views over the same physical units?
- Should sale and rental prices use the same currency?
- Is a zero-price offering valid?
- Should product names be unique, and under what normalization rules?
- Should products support rename, description, and media update methods?
- Should category assignment require an active category?
- Should availability be derived or represented by an explicit offering mode?
- At what order status should inventory be reserved, consumed, or released?
- How are future rental periods checked against existing reservations?
- How are physical units, serial numbers, damage, and maintenance modeled?
- What currency represents existing prices displayed as toman?

These questions should be resolved before treating the rich model as the runtime source of truth.

## Related Documentation

- [Domain capability guide](README.md)
- [Category entity guide](Category.md)
- [Order entity guide](Order.md)
- [OrderItem entity guide](OrderItem.md)
