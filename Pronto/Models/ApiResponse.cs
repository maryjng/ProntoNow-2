namespace Pronto.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}