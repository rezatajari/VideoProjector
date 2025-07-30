namespace Front.DTOs;

public record GeneralResponse<T>(
    bool IsSuccess,
    string Message,
    T Data
    );