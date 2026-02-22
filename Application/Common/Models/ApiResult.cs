using System.Net;

namespace Application.Common.Models
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? DeveloperMessage { get; set; }  // ✅ پیام فنی برای لاگ یا خطای داخلی
        public T? Data { get; set; }
        public HttpStatusCode? StatusCode { get; set; } = HttpStatusCode.OK;

        // ✅ موفق با دیتا
        public static ApiResult<T> Success(T data, string? message = null)
        {
            return new ApiResult<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                StatusCode = HttpStatusCode.OK
            };
        }

        // ✅ موفق بدون دیتا
        public static ApiResult<T> Success(string? message = null)
        {
            return new ApiResult<T>
            {
                IsSuccess = true,
                Message = message,
                StatusCode = HttpStatusCode.OK
            };
        }

        // ✅ خطا (با پیام کاربر و پیام فنی اختیاری)
        public static ApiResult<T> Error(
            string message,
            T? data = default,
            HttpStatusCode? statusCode = HttpStatusCode.BadRequest,
            string? developerMessage = null)
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                DeveloperMessage = developerMessage
            };
        }
    }
}
