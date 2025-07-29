
# Data Transfer Objects (DTOs)

## OrderListDto

The `OrderListDto` class is used for displaying a list of orders. It contains the following properties:

- **OrderId**: The ID of the order.
  - **Required**: Yes
  - **Validation**: None

- **OrderDate**: The date the order was placed.
  - **Required**: Yes
  - **Validation**: None

- **TotalAmount**: The total amount of the order.
  - **Required**: Yes
  - **Validation**: None

- **OrderStatus**: The status of the order.
  - **Required**: Yes
  - **Validation**: None

### Example
```json
{
  "orderId": 123,
  "orderDate": "2023-01-01T00:00:00Z",
  "totalAmount": 150.00,
  "orderStatus": "Pending"
}
```

## AdminOrderUpdateDto

The 

AdminOrderUpdateDto

 class is used for updating order status by an admin. It contains the following properties:

- **OrderId**: The ID of the order.
  - **Required**: Yes
  - **Validation**: None

- **OrderStatus**: The status of the order (e.g., "Pending", "Shipped", "Cancelled").
  - **Required**: Yes
  - **Validation**: None

- **ShippingDate**: The date the order was shipped.
  - **Required**: No
  - **Validation**: None

### Example
```json
{
  "orderId": 123,
  "orderStatus": "Shipped",
  "shippingDate": "2023-01-02T00:00:00Z"
}
```

## CustomerOrderUpdateDto

The 

CustomerOrderUpdateDto

 class is used for updating order information by a customer. It contains the following properties:

- **OrderId**: The ID of the order.
  - **Required**: Yes
  - **Validation**: None

- **ShippingAddress**: The new shipping address for the order.
  - **Required**: Yes
  - **Validation**: None

### Example
```json
{
  "orderId": 123,
  "shippingAddress": "456 New St"
}
```

## OrderAddDto

The 

OrderAddDto

 class is used for adding a new order. It contains the following properties:

- **CustomerId**: The ID of the customer placing the order.
  - **Required**: Yes
  - **Validation**: None

- **TotalAmount**: The total amount of the order.
  - **Required**: Yes
  - **Validation**: None

- **OrderStatus**: The status of the order.
  - **Required**: Yes
  - **Validation**: None

- **ShippingAddress**: The shipping address for the order.
  - **Required**: Yes
  - **Validation**: None

- **OrderDetails**: A list of items in the order.
  - **Required**: Yes
  - **Validation**: None

### Example
```json
{
  "customerId": "cust123",
  "totalAmount": 150.00,
  "orderStatus": "Pending",
  "shippingAddress": "123 Main St",
  "orderDetails": [
    {
      "productId": 1,
      "quantity": 2,
      "price": 50.00,
      "totalAmount": 100.00
    },
    {
      "productId": 2,
      "quantity": 1,
      "price": 50.00,
      "totalAmount": 50.00
    }
  ]
}
```

## OrderDetailDto

The 

OrderDetailDto

 class is used for displaying details of an order item. It contains the following properties:

- **OrderDetailId**: The ID of the order detail.
  - **Required**: Yes
  - **Validation**: None

- **OrderId**: The ID of the order.
  - **Required**: Yes
  - **Validation**: None

- **ProductName**: The name of the product.
  - **Required**: Yes
  - **Validation**: None

- **Quantity**: The quantity of the product.
  - **Required**: Yes
  - **Validation**: None

- **Price**: The price of the product.
  - **Required**: Yes
  - **Validation**: None

- **TotalAmount**: The total amount for the product (quantity * price).
  - **Required**: Yes
  - **Validation**: None

### Example
```json
{
  "orderDetailId": 1,
  "orderId": 123,
  "productName": "Laptop",
  "quantity": 1,
  "price": 999.99,
  "totalAmount": 999.99
}
```

## OrderGetDto

The 

OrderGetDto

 class is used for retrieving detailed information about an order. It contains the following properties:

- **OrderId**: The ID of the order.
  - **Required**: Yes
  - **Validation**: None

- **CustomerId**: The ID of the customer who placed the order.
  - **Required**: Yes
  - **Validation**: None

- **OrderDate**: The date the order was placed.
  - **Required**: Yes
  - **Validation**: None

- **TotalAmount**: The total amount of the order.
  - **Required**: Yes
  - **Validation**: None

- **OrderStatus**: The status of the order.
  - **Required**: Yes
  - **Validation**: None

- **ShippingAddress**: The shipping address for the order.
  - **Required**: Yes
  - **Validation**: None

- **ShippingDate**: The date the order was shipped.
  - **Required**: No
  - **Validation**: None

- **OrderDetails**: A list of items in the order.
  - **Required**: Yes
  - **Validation**: None

### Example
```json
{
  "orderId": 123,
  "customerId": "cust123",
  "orderDate": "2023-01-01T00:00:00Z",
  "totalAmount": 150.00,
  "orderStatus": "Pending",
  "shippingAddress": "123 Main St",
  "shippingDate": "2023-01-02T00:00:00Z",
  "orderDetails": [
    {
      "orderDetailId": 1,
      "orderId": 123,
      "productName": "Laptop",
      "quantity": 1,
      "price": 999.99,
      "totalAmount": 999.99
    }
  ]
}
```
```