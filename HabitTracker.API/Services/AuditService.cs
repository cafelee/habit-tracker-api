using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HabitTracker.API.Services
{
    public class AuditService
    {
        private readonly IConfiguration _config;

        public AuditService(IConfiguration config)
        {
            _config = config;
        }

        public async Task LogAsync(int? userId, string action, string entity, int? entityId, string ipAddress, string detail)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var sql = @"
                INSERT INTO AuditLogs (UserId, Action, Entity, EntityId, Timestamp, IpAddress, Detail)
                VALUES (@UserId, @Action, @Entity, @EntityId, GETDATE(), @IpAddress, @Detail)";
            var param = new
            {
                UserId = userId,
                Action = action,
                Entity = entity,
                EntityId = entityId,
                IpAddress = ipAddress,
                Detail = detail
            };
            await conn.ExecuteAsync(sql, param);
        }
    }
}
