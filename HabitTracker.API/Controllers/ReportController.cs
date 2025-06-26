using Microsoft.AspNetCore.Mvc;
using HabitTracker.API.DTOs;
using HabitTracker.API.Repositories;
using HabitTracker.API.Services;
using HabitTracker.API.Utils;
using Microsoft.AspNetCore.Authorization; // Auth protection
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly HabitRepository _repo;
        private readonly AuditService _auditService;

        public ReportController(HabitRepository repo, AuditService auditService)
        {
            _repo = repo;
            _auditService = auditService;
        }

        [HttpGet("weekly")]
        public async Task<ActionResult<StandardResponse<IEnumerable<WeeklyReportDTO>>>> GetWeeklyReport(
            [FromQuery] int userId,
            [FromQuery] string start,
            [FromQuery] string end)
        {
            if (!DateTime.TryParse(start, out var startDate) || !DateTime.TryParse(end, out var endDate))
            {
                return BadRequest(new StandardResponse<string> { Success = false, Message = "����榡���~" });
            }

            var report = await _repo.GetWeeklyReportAsync(userId, startDate, endDate);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(userId, "Read", "WeeklyReport", null, ipAddress,
                $"���o�ϥΪ� {userId} �q {startDate:yyyy-MM-dd} �� {endDate:yyyy-MM-dd} ���g��");

            return Ok(new StandardResponse<IEnumerable<WeeklyReportDTO>> { Data = report });
        }

        [HttpGet("growth-trend")]
        public async Task<ActionResult<StandardResponse<IEnumerable<GrowthTrendDTO>>>> GetGrowthTrend(
            [FromQuery] int userId,
            [FromQuery] string start,
            [FromQuery] string end)
        {
            if (!DateTime.TryParse(start, out var startDate) || !DateTime.TryParse(end, out var endDate))
            {
                return BadRequest(new StandardResponse<string> { Success = false, Message = "����榡���~" });
            }

            var trend = await _repo.GetGrowthTrendAsync(userId, startDate, endDate);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(userId, "Read", "GrowthTrend", null, ipAddress,
                $"���o�ϥΪ� {userId} �q {startDate:yyyy-MM-dd} �� {endDate:yyyy-MM-dd} �������Ͷ�");

            return Ok(new StandardResponse<IEnumerable<GrowthTrendDTO>> { Data = trend });
        }
    }
}
