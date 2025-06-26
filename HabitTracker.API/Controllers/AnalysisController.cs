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
        private readonly AuditService _auditService;  // �`�J AuditService

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

            // ���o�ϥΪ� IP
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // ������x
            await _auditService.LogAsync(
                userId,
                "Read",
                "BehaviorStyleAnalysis",
                null,
                ipAddress,
                $"���o�Τ� {userId} �q {start:yyyy-MM-dd} �� {end:yyyy-MM-dd} ���欰���R���G");

            return Ok(new StandardResponse<BehaviorStyleDTO> { Data = analysis });
        }
    }
}
