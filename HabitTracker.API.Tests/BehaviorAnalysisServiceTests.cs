using HabitTracker.API.DTOs;
using HabitTracker.API.Services;
using System;
using System.Collections.Generic;
using Xunit;

public class BehaviorAnalysisServiceTests
{
    [Fact]
    public void CalculateMaxStreak_NoCompletedTracks_ReturnsZero()
    {
        var service = new BehaviorAnalysisService();
        var tracks = new List<HabitTrackDTO>();

        var result = service.AnalyzeBehavior(tracks, DateTime.Today, DateTime.Today).MaxStreakDays;

        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateMaxStreak_ConsecutiveDays_ReturnsLongestStreak()
    {
        var service = new BehaviorAnalysisService();
        var tracks = new List<HabitTrackDTO>
        {
            new HabitTrackDTO { TrackDate = new DateTime(2024,1,1), IsCompleted = true },
            new HabitTrackDTO { TrackDate = new DateTime(2024,1,2), IsCompleted = true },
            new HabitTrackDTO { TrackDate = new DateTime(2024,1,4), IsCompleted = true },
            new HabitTrackDTO { TrackDate = new DateTime(2024,1,5), IsCompleted = true }
        };

        var result = service.AnalyzeBehavior(tracks, new DateTime(2024,1,1), new DateTime(2024,1,5)).MaxStreakDays;

        Assert.Equal(2, result);
    }
}
