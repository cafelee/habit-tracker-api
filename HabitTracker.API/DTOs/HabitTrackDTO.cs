namespace HabitTracker.API.DTOs
{
    public class HabitTrackDTO
    {
        public DateTime TrackDate { get; set; } = DateTime.Today;
        public bool IsCompleted { get; set; } = true;
    }
}
