
# Data Transfer Objects (DTOs)

## ProductSearchDto

The `ProductSearchDto` class is used for searching products. It contains the following properties:

- **SearchTerm**: The keyword for search.
  - **Required**: Yes
  - **Validation**: None

- **CategoryId**: The ID of the category to filter by.
  - **Required**: No
  - **Validation**: None

- **MinPrice**: The minimum price filter.
  - **Required**: No
  - **Validation**: None

- **MaxPrice**: The maximum price filter.
  - **Required**: No
  - **Validation**: None

### Example
```json
{
  "searchTerm": "laptop",
  "categoryId": 1,
  "minPrice": 500.00,
  "maxPrice": 1500.00
}
```

## GetProductDto

The 

GetProductDto

 class is used for retrieving a product by its ID. It contains the following property:

- **ProductId**: The ID of the product.
  - **Required**: Yes
  - **Validation**: None

### Example
```json
{
  "productId": 123
}
```

## ProductDetailDto

The 

ProductDetailDto

 class is used for displaying detailed information about a product. It contains the following properties:

- **ProductId**: The ID of the product.
  - **Required**: Yes
  - **Validation**: None

- **Name**: The name of the product.
  - **Required**: No
  - **Validation**: None

- **Description**: The description of the product.
  - **Required**: No
  - **Validation**: None

- **Price**: The price of the product.
  - **Required**: Yes
  - **Validation**: None

- **StockQuantity**: The quantity of the product in stock.
  - **Required**: Yes
  - **Validation**: None

- **ImageUrl**: The URL of the product's image.
  - **Required**: No
  - **Validation**: None

- **CategoryName**: The name of the category the product belongs to.
  - **Required**: No
  - **Validation**: None

### Example
```json
{
  "productId": 123,
  "name": "Laptop",
  "description": "A high-performance laptop.",
  "price": 999.99,
  "stockQuantity": 50,
  "imageUrl": "/images/laptop.jpg",
  "categoryName": "Electronics"
}
```

## ProductListDto

The 

ProductListDto

 class is used for displaying a list of products. It contains the following properties:

- **ProductId**: The ID of the product.
  - **Required**: Yes
  - **Validation**: None

- **Name**: The name of the product.
  - **Required**: No
  - **Validation**: None

- **Price**: The price of the product.
  - **Required**: Yes
  - **Validation**: None

- **ImageUrl**: The URL of the product's image.
  - **Required**: No
  - **Validation**: None

- **CategoryName**: The name of the category the product belongs to.
  - **Required**: No
  - **Validation**: None

### Example
```json
{
  "productId": 123,
  "name": "Laptop",
  "price": 999.99,
  "imageUrl": "/images/laptop.jpg",
  "categoryName": "Electronics"
}
```
```