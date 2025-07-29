
# Shopping Cart API - DTO Documentation

## Overview
This document outlines the Data Transfer Objects (DTOs) used for the shopping cart functionality in the Video Projector application. These DTOs serve as the communication format between the API and the user interface (UI).

### DTOs:
- **ShoppingCartDto**: Represents a shopping cart, including details such as the customer and a list of items in the cart.
- **ShoppingCartItemDto**: Represents a single item in the shopping cart, including product information and quantity.

---

## 1. ShoppingCartDto

The `ShoppingCartDto` is used to transfer information about a shopping cart, including the customer's details and the list of items in the cart.

### Properties:

- **ShoppingCartId** (`int`):  
  The unique identifier for the shopping cart.
  
- **CustomerId** (`string`):  
  The unique identifier of the customer (from Identity User) who owns the shopping cart.

- **CreatedDate** (`DateTime`):  
  The date and time when the shopping cart was created.

- **Items** (`List<ShoppingCartItemDto>`):  
  A collection of items currently in the shopping cart. Each item is represented by a `ShoppingCartItemDto`.

### Example:
```json
{
  "shoppingCartId": 1,
  "customerId": "customer123",
  "createdDate": "2025-01-18T10:00:00Z",
  "items": [
    {
      "shoppingCartItemId": 1,
      "productId": 101,
      "productName": "Projector A",
      "quantity": 2,
      "price": 499.99,
      "totalPrice": 999.98
    },
    {
      "shoppingCartItemId": 2,
      "productId": 102,
      "productName": "Projector B",
      "quantity": 1,
      "price": 299.99,
      "totalPrice": 299.99
    }
  ]
}
```

---

## 2. ShoppingCartItemDto

The `ShoppingCartItemDto` represents an individual item in the shopping cart. This DTO includes product details and quantity information.

### Properties:

- **ShoppingCartItemId** (`int`):  
  The unique identifier for the shopping cart item.

- **ProductId** (`int`):  
  The unique identifier of the product being added to the cart.

- **ProductName** (`string`):  
  The name of the product.

- **Quantity** (`int`):  
  The quantity of the product being added to the cart.

- **Price** (`decimal`):  
  The price of the product at the time it was added to the cart.

- **TotalPrice** (`decimal`):  
  The total price for the item in the cart, calculated as `Quantity * Price`.

### Example:
```json
{
  "shoppingCartItemId": 1,
  "productId": 101,
  "productName": "Projector A",
  "quantity": 2,
  "price": 499.99,
  "totalPrice": 999.98
}
```

---

## Data Flow

1. **Adding Items to the Cart**:  
   When a customer adds items to their shopping cart, the UI will send a request containing a list of `ShoppingCartItemDto` objects. The server will process the items, update the shopping cart, and return the updated cart as a `ShoppingCartDto`.

2. **Modifying Items in the Cart**:  
   If the customer modifies the quantity or removes an item, the server will receive updated `ShoppingCartItemDto` objects and update the cart accordingly.

3. **Viewing the Cart**:  
   When the customer wants to view their cart, the server will return the `ShoppingCartDto`, which includes the details of the items in the cart.

4. **Calculating Total**:  
   The total price of each item is calculated using the `Quantity` and `Price` fields. The overall cart total is calculated based on the sum of `TotalPrice` for each item.

---

## Conclusion

These DTOs provide a clear and concise way to communicate between the shopping cart service and the UI. By using these DTOs, the application can efficiently manage the shopping cart data while keeping the communication lightweight and focused.

---

