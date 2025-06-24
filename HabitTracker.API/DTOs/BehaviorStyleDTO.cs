namespace HabitTracker.API.DTOs
{
    public class BehaviorStyleDTO
    {
        public string StyleName { get; set; } = string.Empty;        // 行為風格名稱
        public string Description { get; set; } = string.Empty;      // 風格說明
        public decimal CompletionRate { get; set; }                  // 完成率
        public double AverageTrackHour { get; set; }                 // 平均打卡時間（小時）
        public double TrackTimeStdDev { get; set; }                  // 打卡時間標準差（小時）
        public int MaxStreakDays { get; set; }                        // 最大連續完成天數
        public decimal AverageDelayHours { get; set; }               // 平均打卡延遲（小時）
    }
}
