using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HabitTracker.API.Services
{
    public class LogMaintenanceService
    {
        private readonly IConfiguration _config;

        public LogMaintenanceService(IConfiguration config)
        {
            _config = config;
        }

        // �R���W�L���w�Ѽƪ���x�A�w�]30��
        public async Task CleanupOldLogsAsync(int daysThreshold = 30)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var sql = @"DELETE FROM AuditLogs WHERE Timestamp < DATEADD(DAY, -@Days, GETDATE())";
            await conn.ExecuteAsync(sql, new { Days = daysThreshold });
        }
    }
}
