using HabitTracker.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

public class BehaviorAnalysisService
{
    public BehaviorStyleDTO AnalyzeBehavior(IEnumerable<HabitTrackDTO> tracks, DateTime startDateParameter, DateTime endDateParameter)
    {
        if (tracks == null || !tracks.Any())
            return new BehaviorStyleDTO { StyleName = "�L���", Description = "�L������Ƥ��R" };

        var totalDays = (endDateParameter - startDateParameter).TotalDays + 1;
        var completedCount = tracks.Count(t => t.IsCompleted);
        decimal completionRate = totalDays == 0 ? 0 : (decimal)completedCount / (decimal)totalDays;

        // �������d�ɶ��]�p�ɡ^
        var trackHours = tracks.Select(t => t.TrackDate.TimeOfDay.TotalHours);
        double avgHour = trackHours.Average();

        // �зǮt
        double stdDev = Math.Sqrt(trackHours.Average(v => Math.Pow(v - avgHour, 2)));

        // �̤j�s�򧹦��Ѽ�
        int maxStreak = CalculateMaxStreak(tracks);

        // �������d����]�p�ɡ^�A�o�̰��]�w�����d�ɶ��O���W8�I
        var delays = tracks.Where(t => t.IsCompleted)
            .Select(t => (t.TrackDate.TimeOfDay.TotalHours - 8.0))
            .Select(h => h < 0 ? 0 : h);  // ���p����8�I��
        decimal avgDelay = delays.Any() ? (decimal)delays.Average() : 0;

        // ²������޿�
        string styleName;
        string description;

        if (completionRate > 0.8m && stdDev < 2 && avgDelay < 1)
        {
            styleName = "�W�߫�";
            description = "�A����í�w�����d�ߺD�A�ɶ��T�w�A�����v���I";
        }
        else if (completionRate > 0.5m && avgDelay >= 1)
        {
            styleName = "�쩵��";
            description = "�A�|�������d�A���ɱ`�쩵�A���սվ�ɶ��a�I";
        }
        else if (completionRate > 0.3m)
        {
            styleName = "������";
            description = "���d�����W�ߡA�����ʧ����A����V�O�I";
        }
        else
        {
            styleName = "��o��";
            description = "���d�ܤ�í�w�A��ĳ�]�w�������U�ۤv�C";
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
