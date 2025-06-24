namespace HabitTracker.API.DTOs
{
    public class GrowthTrendDTO
    {
        public DateTime Date { get; set; }
        public decimal DailyCompletionRate { get; set; }  // 0~1 ªº¤ñ¨Ò
    }
}
