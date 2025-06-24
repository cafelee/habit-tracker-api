namespace HabitTracker.API.DTOs
{
    public class BehaviorStyleDTO
    {
        public string StyleName { get; set; } = string.Empty;        // �欰����W��
        public string Description { get; set; } = string.Empty;      // ���满��
        public decimal CompletionRate { get; set; }                  // �����v
        public double AverageTrackHour { get; set; }                 // �������d�ɶ��]�p�ɡ^
        public double TrackTimeStdDev { get; set; }                  // ���d�ɶ��зǮt�]�p�ɡ^
        public int MaxStreakDays { get; set; }                        // �̤j�s�򧹦��Ѽ�
        public decimal AverageDelayHours { get; set; }               // �������d����]�p�ɡ^
    }
}
