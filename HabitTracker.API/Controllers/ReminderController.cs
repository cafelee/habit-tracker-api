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
    [Route("api/reminders")]
    [Authorize]
    public class ReminderController : ControllerBase
    {
        private readonly HabitRepository _repo;
        private readonly AuditService _auditService;

        public ReminderController(HabitRepository repo, AuditService auditService)
        {
            _repo = repo;
            _auditService = auditService;
        }

        [HttpGet("priorities")]
        public async Task<ActionResult<StandardResponse<IEnumerable<ReminderPriorityDTO>>>> GetReminderPriorities(
            [FromQuery] int userId,
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var list = await _repo.GetReminderPrioritiesAsync(userId, start, end);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(userId, "Read", "ReminderPriorities", null, ipAddress,
                $"���o�ϥΪ� {userId} �q {start:yyyy-MM-dd} �� {end:yyyy-MM-dd} �������u������");

            return Ok(new StandardResponse<IEnumerable<ReminderPriorityDTO>> { Data = list });
        }
    }
}
