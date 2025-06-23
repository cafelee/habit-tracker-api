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

    }
}
