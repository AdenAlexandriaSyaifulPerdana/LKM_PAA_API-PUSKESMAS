namespace LKM_PAA.Helpers
{
    public class ApiResponse<T>
    {
        public string Status { get; set; }
        public T Data { get; set; }
        public object Meta { get; set; }
    }

    public class ApiError
    {
        public string Status { get; set; } = "error";
        public string Message { get; set; }
    }
}