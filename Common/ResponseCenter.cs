namespace VideoProjector.Common
{
    /// <summary>
    /// Represents a standardized response structure for API responses.
    /// </summary>
    /// <typeparam name="T">The type of the data being returned in the response.</typeparam>
    public class ResponseCenter<T>
    {//TODO: change structure of responce cfenter becuase need to issuccess
        /// <summary>
        /// Gets or sets the status of the response (e.g., "Success", "Error").
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets a human-readable message providing context for the response.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the data being returned in the response.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets an optional error code providing more specific information about an error.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets a list of validation errors, if any.
        /// </summary>
        public List<string> ValidationErrors { get; set; }

        /// <summary>
        /// Gets or sets additional metadata related to the response.
        /// </summary>
        public object Metadata { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseCenter{T}"/> class.
        /// </summary>
        public ResponseCenter()
        {
            ValidationErrors = new List<string>();
        }
    }

    /// <summary>
    /// Provides static methods for creating standardized response objects.
    /// </summary>
    public static class ResponseCenter
    {
        /// <summary>
        /// Creates a success response with the specified data and message.
        /// </summary>
        /// <typeparam name="T">The type of the data being returned in the response.</typeparam>
        /// <param name="data">The data to include in the response.</param>
        /// <param name="message">An optional message providing context for the response.</param>
        /// <returns>A <see cref="ResponseCenter{T}"/> object representing a success response.</returns>
        public static ResponseCenter<T> CreateSuccessResponse<T>(T data, string message = "Operation successful.")
        {
            return new ResponseCenter<T>
            {
                Status = "Success",
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates an error response with the specified message, error code, and validation errors.
        /// </summary>
        /// <typeparam name="T">The type of the data being returned in the response (typically not used for error responses).</typeparam>
        /// <param name="message">A message providing context for the error.</param>
        /// <param name="errorCode">An optional error code providing more specific information about the error.</param>
        /// <param name="validationErrors">An optional list of validation errors.</param>
        /// <returns>A <see cref="ResponseCenter{T}"/> object representing an error response.</returns>
        public static ResponseCenter<T> CreateErrorResponse<T>(string message, string errorCode = null, List<string> validationErrors = null)
        {
            return new ResponseCenter<T>
            {
                Status = "Error",
                Message = message,
                ErrorCode = errorCode,
                ValidationErrors = validationErrors ?? new List<string>()
            };
        }
    }
}