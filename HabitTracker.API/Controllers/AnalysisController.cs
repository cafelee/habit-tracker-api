using HabitTracker.API.DTOs;
using HabitTracker.API.Repositories;
using HabitTracker.API.Services;
using HabitTracker.API.Utils;
using Microsoft.AspNetCore.Authorization; // Auth protection
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    [Authorize]
    public class AnalysisController : ControllerBase
    {
        private readonly HabitRepository _repo;
        private readonly BehaviorAnalysisService _analysisService;
        private readonly AuditService _auditService;  // 注入 AuditService

        public AnalysisController(HabitRepository repo, AuditService auditService)
        {
            _repo = repo;
            _auditService = auditService;
            _analysisService = new BehaviorAnalysisService();
        }

        [HttpGet("style")]
        public async Task<ActionResult<StandardResponse<BehaviorStyleDTO>>> GetBehaviorStyle(
            [FromQuery] int userId,
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var tracks = await _repo.GetHabitTracksByUserAsync(userId, start, end);
            var analysis = _analysisService.AnalyzeBehavior(tracks, start, end);

            // 取得使用者 IP
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // 紀錄日誌
            await _auditService.LogAsync(
                userId,
                "Read",
                "BehaviorStyleAnalysis",
                null,
                ipAddress,
                $"取得用戶 {userId} 從 {start:yyyy-MM-dd} 到 {end:yyyy-MM-dd} 的行為分析結果");

            return Ok(new StandardResponse<BehaviorStyleDTO> { Data = analysis });
        }
    }
}
