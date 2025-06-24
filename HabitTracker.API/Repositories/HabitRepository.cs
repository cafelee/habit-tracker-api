using Dapper;
using HabitTracker.API.DTOs;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace HabitTracker.API.Repositories
{
    public class HabitRepository
    {
        private readonly IConfiguration _config;

        public HabitRepository(IConfiguration config)
        {
            _config = config;
        }

        // 建立習慣
        public async Task<int> CreateHabitAsync(HabitCreateDTO dto)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var json = JsonSerializer.Serialize(dto);

            var param = new DynamicParameters();
            param.Add("@InputJson", json, DbType.String);

            var habitId = await conn.ExecuteScalarAsync<int>(
                "sp_Habit_Create",
                param,
                commandType: CommandType.StoredProcedure
            );

            return habitId;
        }

        public async Task<IEnumerable<HabitCreateDTO>> GetAllHabitsAsync()
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await conn.QueryAsync<HabitCreateDTO>("sp_Habit_GetAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<HabitCreateDTO?> GetHabitByIdAsync(int id)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await conn.QueryFirstOrDefaultAsync<HabitCreateDTO>(
                "sp_Habit_GetById",
                new { HabitId = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateHabitAsync(int id, HabitUpdateDTO dto)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var json = JsonSerializer.Serialize(dto);
            var param = new DynamicParameters();
            param.Add("@HabitId", id);
            param.Add("@InputJson", json);

            await conn.ExecuteAsync("sp_Habit_Update", param, commandType: CommandType.StoredProcedure);
            return true;
        }

        public async Task<bool> DeleteHabitAsync(int id)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.ExecuteAsync("sp_Habit_Delete", new { HabitId = id }, commandType: CommandType.StoredProcedure);
            return true;
        }

        // 打卡習慣
        public async Task<bool> TrackHabitAsync(int habitId, HabitTrackDTO dto)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var json = JsonSerializer.Serialize(dto);

            var param = new DynamicParameters();
            param.Add("@HabitId", habitId);
            param.Add("@InputJson", json);

            try
            {
                await conn.ExecuteAsync(
                    "sp_Habit_Track",
                    param,
                    commandType: CommandType.StoredProcedure
                );
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 取得習慣列表
        public async Task<IEnumerable<HabitTrackRecordDTO>> GetHabitTracksAsync(int habitId, DateTime start, DateTime end)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var param = new DynamicParameters();
            param.Add("@HabitId", habitId);
            param.Add("@StartDate", start);
            param.Add("@EndDate", end);

            var result = await conn.QueryAsync<HabitTrackRecordDTO>(
                "sp_Habit_GetTracksInRange",
                param,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<IEnumerable<WeeklyReportDTO>> GetWeeklyReportAsync(int userId, DateTime start, DateTime end)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@StartDate", start);
            param.Add("@EndDate", end);

            var result = await conn.QueryAsync<WeeklyReportDTO>(
                "sp_Report_WeeklySummary",
                param,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<IEnumerable<ReminderPriorityDTO>> GetReminderPrioritiesAsync(int userId, DateTime start, DateTime end)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@StartDate", start);
            param.Add("@EndDate", end);

            var result = await conn.QueryAsync<ReminderPriorityDTO>(
                "sp_Habit_ReminderPriority",
                param,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        // HabitRepository
        public async Task<IEnumerable<HabitTrackDTO>> GetHabitTracksByUserAsync(int userId, DateTime start, DateTime end)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@StartDate", start);
            param.Add("@EndDate", end);

            var sql = @"
                SELECT HT.HABIT_ID, HT.TRACK_DATE, HT.IS_COMPLETED, H.FREQUENCY, H.TITLE
                FROM HABIT_TRACK HT
                INNER JOIN HABIT H ON HT.HABIT_ID = H.HABIT_ID
                WHERE H.USER_ID = @UserId
                AND HT.TRACK_DATE BETWEEN @StartDate AND @EndDate
                ORDER BY HT.TRACK_DATE";

            return await conn.QueryAsync<HabitTrackDTO>(sql, param);
        }

        // 取得成長趨勢
        public async Task<IEnumerable<GrowthTrendDTO>> GetGrowthTrendAsync(int userId, DateTime start, DateTime end)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@StartDate", start);
            param.Add("@EndDate", end);

            return await conn.QueryAsync<GrowthTrendDTO>(
                "sp_Report_GrowthTrend",
                param,
                commandType: CommandType.StoredProcedure);
        }

    }
}
