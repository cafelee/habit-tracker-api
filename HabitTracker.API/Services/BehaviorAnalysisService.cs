using HabitTracker.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

public class BehaviorAnalysisService
{
    public BehaviorStyleDTO AnalyzeBehavior(IEnumerable<HabitTrackDTO> tracks, DateTime startDateParameter, DateTime endDateParameter)
    {
        if (tracks == null || !tracks.Any())
            return new BehaviorStyleDTO { StyleName = "無資料", Description = "無足夠資料分析" };

        var totalDays = (endDateParameter - startDateParameter).TotalDays + 1;
        var completedCount = tracks.Count(t => t.IsCompleted);
        decimal completionRate = totalDays == 0 ? 0 : (decimal)completedCount / (decimal)totalDays;

        // 平均打卡時間（小時）
        var trackHours = tracks.Select(t => t.TrackDate.TimeOfDay.TotalHours);
        double avgHour = trackHours.Average();

        // 標準差
        double stdDev = Math.Sqrt(trackHours.Average(v => Math.Pow(v - avgHour, 2)));

        // 最大連續完成天數
        int maxStreak = CalculateMaxStreak(tracks);

        // 平均打卡延遲（小時），這裡假設預期打卡時間是早上8點
        var delays = tracks.Where(t => t.IsCompleted)
            .Select(t => (t.TrackDate.TimeOfDay.TotalHours - 8.0))
            .Select(h => h < 0 ? 0 : h);  // 不計早於8點的
        decimal avgDelay = delays.Any() ? (decimal)delays.Average() : 0;

        // 簡單分類邏輯
        string styleName;
        string description;

        if (completionRate > 0.8m && stdDev < 2 && avgDelay < 1)
        {
            styleName = "規律型";
            description = "你有很穩定的打卡習慣，時間固定，完成率高！";
        }
        else if (completionRate > 0.5m && avgDelay >= 1)
        {
            styleName = "拖延型";
            description = "你會完成打卡，但時常拖延，嘗試調整時間吧！";
        }
        else if (completionRate > 0.3m)
        {
            styleName = "間歇型";
            description = "打卡較不規律，間歇性完成，持續努力！";
        }
        else
        {
            styleName = "突發型";
            description = "打卡很不穩定，建議設定提醒幫助自己。";
        }

        return new BehaviorStyleDTO
        {
            StyleName = styleName,
            Description = description,
            CompletionRate = Math.Round(completionRate, 2),
            AverageTrackHour = Math.Round(avgHour, 2),
            TrackTimeStdDev = Math.Round(stdDev, 2),
            MaxStreakDays = maxStreak,
            AverageDelayHours = Math.Round(avgDelay, 2)
        };
    }

    private int CalculateMaxStreak(IEnumerable<HabitTrackDTO> tracks)
    {
        var ordered = tracks
            .Where(t => t.IsCompleted)
            .Select(t => t.TrackDate.Date)
            .Distinct()
            .OrderBy(d => d)
            .ToList();

        int maxStreak = 0, currentStreak = 1;

        for (int i = 1; i < ordered.Count; i++)
        {
            if ((ordered[i] - ordered[i - 1]).Days == 1)
                currentStreak++;
            else
            {
                if (currentStreak > maxStreak)
                    maxStreak = currentStreak;
                currentStreak = 1;
            }
        }

        if (currentStreak > maxStreak)
            maxStreak = currentStreak;

        return maxStreak;
    }
}
