namespace HabitTracker.API.DTOs
{
    public class WeeklyReportDTO
    {
        public int HabitId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CompletedCount { get; set; }
        public int TotalDays { get; set; }
        public decimal CompletionRate { get; set; }  // �ʤ���A�ҡG75.00
    }
}
