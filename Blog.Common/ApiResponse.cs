using System.Net;

namespace Blog.Common
{
	public class ApiResponse<TData>
	{
		public bool Success { get; set; }

		public int StatusCode { get; set; }

		public string Message { get; set; } = string.Empty;

		public TData? Data { get; set; }

		public object? Errors { get; set; }

		public DateTime Timestamp { get; set; } = DateTime.UtcNow;

		public static ApiResponse<TData> Create(
			bool success,
			int statusCode,
			string message,
			TData? data = default,
			object? errors = null)
		{
			return new ApiResponse<TData>
			{
				Success = success,
				StatusCode = statusCode,
				Message = message,
				Data = data,
				Errors = errors
			};
		}

		#region Success Responses

		public static ApiResponse<TData> Ok(
			TData data,
			string message = "Request completed successfully")
			=> Create(true, (int)HttpStatusCode.OK, message, data);

		public static ApiResponse<TData> Created(
			TData data,
			string message = "Resource created successfully")
			=> Create(true, (int)HttpStatusCode.Created, message, data);

		public static ApiResponse<TData> CreatedAt(
			TData data,
			string message = "Resource created successfully")
			=> Created(data, message);

		public static ApiResponse<TData> Accepted(
			TData? data = default,
			string message = "Request accepted")
			=> Create(true, (int)HttpStatusCode.Accepted, message, data);

		public static ApiResponse<TData> NoContent(
			string message = "Operation completed successfully")
			=> Create(true, (int)HttpStatusCode.NoContent, message);

		#endregion

		#region Client Errors

		public static ApiResponse<TData> BadRequest(
			string message = "Bad request",
			object? errors = null)
			=> Create(false, (int)HttpStatusCode.BadRequest, message, errors: errors);

		public static ApiResponse<TData> Unauthorized(
			string message = "Unauthorized")
			=> Create(false, (int)HttpStatusCode.Unauthorized, message);

		public static ApiResponse<TData> Forbidden(
			string message = "Access denied")
			=> Create(false, (int)HttpStatusCode.Forbidden, message);

		public static ApiResponse<TData> NotFound(
			string message = "Resource not found")
			=> Create(false, (int)HttpStatusCode.NotFound, message);

		public static ApiResponse<TData> Conflict(
			string message = "Conflict occurred")
			=> Create(false, (int)HttpStatusCode.Conflict, message);

		public static ApiResponse<TData> ValidationError(
			object errors,
			string message = "Validation failed")
			=> Create(false, 422, message, errors: errors);

		#endregion

		#region Server Errors

		public static ApiResponse<TData> InternalServerError(
			string message = "Internal server error",
			object? errors = null)
			=> Create(false, (int)HttpStatusCode.InternalServerError, message, errors: errors);

		public static ApiResponse<TData> ServiceUnavailable(
			string message = "Service unavailable")
			=> Create(false, (int)HttpStatusCode.ServiceUnavailable, message);

		public static ApiResponse<TData> Error(
			int statusCode,
			string message,
			object? errors = null)
			=> Create(false, statusCode, message, errors: errors);

		#endregion
	}
}
