# Data Transfer Objects (DTOs)

## LoginDto

The `LoginDto` class is used for user login requests. It contains the following properties:

- **Email**: The user's email address.
  - **Required**: Yes
  - **Validation**: Must be a valid email format.
  - **Error Message**: "Email is required.", "Invalid email format."

- **Password**: The user's password.
  - **Required**: Yes
  - **Validation**: None
  - **Error Message**: "Password is required."

### Example
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

## RegisterDto

The `RegisterDto` class is used for user registration requests. It contains the following properties:

- **Username**: The user's username.
  - **Required**: No
  - **Validation**: Maximum length of 50 characters.
  - **Error Message**: "Username cannot be longer than 50 characters."

- **Email**: The user's email address.
  - **Required**: Yes
  - **Validation**: Must be a valid email format.
  - **Error Message**: "Email is required.", "Invalid email format."

- **Password**: The user's password.
  - **Required**: Yes
  - **Validation**: Minimum length of 6 characters.
  - **Error Message**: "Password is required.", "Password must be at least 6 characters long."

- **ConfirmPassword**: The user's password confirmation.
  - **Required**: Yes
  - **Validation**: Must match the `Password` property.
  - **Error Message**: "Confirm Password is required.", "Passwords do not match."

- **Address**: The user's address.
  - **Required**: No
  - **Validation**: Maximum length of 250 characters.
  - **Error Message**: "Address cannot be longer than 250 characters."

- **Gender**: The user's gender.
  - **Required**: Yes
  - **Validation**: None
  - **Error Message**: "Gender is required."

### Example
```json
{
  "username": "user123",
  "email": "user@example.com",
  "password": "password123",
  "confirmPassword": "password123",
  "address": "123 Main St",
  "gender": "Male"
}
```