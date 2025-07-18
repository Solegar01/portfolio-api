namespace PortfolioApi.Models
{
    public class ApiResponseViewModel<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponseViewModel() { }

        public ApiResponseViewModel(bool success, string? message = null, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponseViewModel<T> Ok(T data, string? message = null) =>
            new ApiResponseViewModel<T>(true, message, data);

        public static ApiResponseViewModel<T> Fail(string message) =>
            new ApiResponseViewModel<T>(false, message, default);
    }
}
