namespace HabitTracker.API.Utils
{
    public class StandardResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "OK";
        public T? Data { get; set; }
    }
}
