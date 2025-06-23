namespace HabitTracker.API.DTOs
{
    public class ReminderPriorityDTO
    {
        public int HabitId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal UncompletedRate { get; set; }
        public int FrequencyWeight { get; set; }
        public int DaysSinceLastTrack { get; set; }
        public int StreakDays { get; set; }
        public decimal ReminderScore { get; set; }
    }
}
