using Dapper;
using System.Data;
using System.Data.SqlClient;
using HabitTracker.API.DTOs;

namespace HabitTracker.API.Repositories
{
    public class HabitRepository
    {
        private readonly IConfiguration _config;
        public HabitRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> CreateHabitAsync(HabitCreateDTO dto)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var json = System.Text.Json.JsonSerializer.Serialize(dto);

            var param = new DynamicParameters();
            param.Add("@InputJson", json, DbType.String);

            var habitId = await conn.ExecuteScalarAsync<int>(
                "sp_Habit_Create",
                param,
                commandType: CommandType.StoredProcedure
            );

            return habitId;
        }
    }
}
