# ProfileDto

The `ProfileDto` class is used for displaying user profile information. It contains the following properties:

- **Username**: The user's username.
  - **Required**: No
  - **Validation**: Maximum length of 50 characters.
  - **Error Message**: "Username cannot be longer than 50 characters."

- **RegistrationDate**: The date the user registered.
  - **Required**: Yes
  - **Validation**: None

- **ProfilePicture**: The user's profile picture URL.
  - **Required**: No
  - **Validation**: None

### Example
```json
{
  "username": "user123",
  "registrationDate": "2023-01-01T00:00:00Z",
  "profilePicture": "/images/profile.jpg"
}
```

# EditDto

The `EditDto` class is used for editing user profile information. It contains the following properties:

- **Username**: The user's username.
  - **Required**: No
  - **Validation**: Maximum length of 50 characters.
  - **Error Message**: "Username cannot be longer than 50 characters."

- **Email**: The user's email address.
  - **Required**: Yes
  - **Validation**: Must be a valid email format.
  - **Error Message**: "Email is required.", "Invalid email format."

- **Address**: The user's address.
  - **Required**: No
  - **Validation**: None

- **ProfilePicture**: The user's profile picture file.
  - **Required**: No
  - **Validation**: None

- **CurrentProfilePicturePath**: The path to the user's current profile picture.
  - **Required**: No
  - **Validation**: None

### Example
```json
{
  "username": "user123",
  "email": "user@example.com",
  "address": "123 Main St",
  "profilePicture": "profile.jpg",
  "currentProfilePicturePath": "/images/profile.jpg"
}
```

# UpdatePasswordDto

The `UpdatePasswordDto` class is used for updating user passwords. It contains the following properties:

- **CurrentPassword**: The user's current password.
  - **Required**: Yes
  - **Validation**: Minimum length of 6 characters.
  - **Error Message**: "Current Password is required.", "Password must be at least 6 characters long."

- **NewPassword**: The user's new password.
  - **Required**: Yes
  - **Validation**: Minimum length of 6 characters.
  - **Error Message**: "New Password is required.", "Password must be at least 6 characters long."

### Example
```json
{
  "currentPassword": "oldpassword123",
  "newPassword": "newpassword123"
}
```
```