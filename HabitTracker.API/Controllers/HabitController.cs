using Microsoft.AspNetCore.Mvc;
using HabitTracker.API.DTOs;
using HabitTracker.API.Repositories;
using HabitTracker.API.Utils;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitController : ControllerBase
    {
        private readonly HabitRepository _repo;

        public HabitController(HabitRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<ActionResult<StandardResponse<int>>> CreateHabit([FromBody] HabitCreateDTO dto)
        {
            var id = await _repo.CreateHabitAsync(dto);
            return Ok(new StandardResponse<int>
            {
                Data = id,
                Message = "習慣建立成功"
            });
        }

        [HttpPost("{id}/track")]
        public async Task<ActionResult<StandardResponse<string>>> TrackHabit(int id, [FromBody] HabitTrackDTO dto)
        {
            var success = await _repo.TrackHabitAsync(id, dto);
            if (!success) return BadRequest(new StandardResponse<string> { Success = false, Message = "打卡失敗" });

            return Ok(new StandardResponse<string> { Message = "打卡成功" });
        }

        [HttpGet("{id}/tracks")]
        public async Task<ActionResult<StandardResponse<IEnumerable<HabitTrackRecordDTO>>>> GetTracks(
    int id, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var tracks = await _repo.GetHabitTracksAsync(id, start, end);
            return Ok(new StandardResponse<IEnumerable<HabitTrackRecordDTO>>
            {
                Data = tracks
            });
        }

    }
}
