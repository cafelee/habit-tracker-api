// ReportController.cs
using Microsoft.AspNetCore.Mvc;
using HabitTracker.API.DTOs;
using HabitTracker.API.Repositories;
using HabitTracker.API.Utils;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly HabitRepository _repo;

        public ReportController(HabitRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("weekly")]
        public async Task<ActionResult<StandardResponse<IEnumerable<WeeklyReportDTO>>>> GetWeeklyReport(
            [FromQuery] int userId,
            [FromQuery] string start,
            [FromQuery] string end)
        {
            if (!DateTime.TryParse(start, out var startDate) || !DateTime.TryParse(end, out var endDate))
            {
                return BadRequest(new StandardResponse<string> { Success = false, Message = "日期格式錯誤" });
            }

            var report = await _repo.GetWeeklyReportAsync(userId, startDate, endDate);
            return Ok(new StandardResponse<IEnumerable<WeeklyReportDTO>> { Data = report });
        }

    }
}
