namespace HabitTracker.API.DTOs
{
    public class HabitUpdateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Frequency { get; set; } = "daily";
    }
}
