using HabitTracker.API.DTOs;
using HabitTracker.API.Repositories;
using HabitTracker.API.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    public class AnalysisController : ControllerBase
    {
        private readonly HabitRepository _repo;
        private readonly BehaviorAnalysisService _analysisService;

        public AnalysisController(HabitRepository repo)
        {
            _repo = repo;
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
            return Ok(new StandardResponse<BehaviorStyleDTO> { Data = analysis });
        }
    }
}
