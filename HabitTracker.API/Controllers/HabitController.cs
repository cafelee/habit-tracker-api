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
                Message = "�ߺD�إߦ��\"
            });
        }
        // ���o�����ߺD
        [HttpGet]
        public async Task<ActionResult<StandardResponse<IEnumerable<HabitCreateDTO>>>> GetAllHabits()
        {
            var habits = await _repo.GetAllHabitsAsync();
            return Ok(new StandardResponse<IEnumerable<HabitCreateDTO>> { Data = habits });
        }

        // ���o�浧�ߺD
        [HttpGet("{id}")]
        public async Task<ActionResult<StandardResponse<HabitCreateDTO>>> GetHabitById(int id)
        {
            var habit = await _repo.GetHabitByIdAsync(id);
            if (habit == null)
                return NotFound(new StandardResponse<HabitCreateDTO> { Success = false, Message = "�䤣����" });

            return Ok(new StandardResponse<HabitCreateDTO> { Data = habit });
        }

        // ��s�ߺD
        [HttpPut("{id}")]
        public async Task<ActionResult<StandardResponse<string>>> UpdateHabit(int id, [FromBody] HabitUpdateDTO dto)
        {
            await _repo.UpdateHabitAsync(id, dto);
            return Ok(new StandardResponse<string> { Message = "��s���\" });
        }

        // �R���ߺD
        [HttpDelete("{id}")]
        public async Task<ActionResult<StandardResponse<string>>> DeleteHabit(int id)
        {
            await _repo.DeleteHabitAsync(id);
            return Ok(new StandardResponse<string> { Message = "�R�����\" });
        }

        [HttpPost("{id}/track")]
        public async Task<ActionResult<StandardResponse<string>>> TrackHabit(int id, [FromBody] HabitTrackDTO dto)
        {
            var success = await _repo.TrackHabitAsync(id, dto);
            if (!success) return BadRequest(new StandardResponse<string> { Success = false, Message = "���d����" });

            return Ok(new StandardResponse<string> { Message = "���d���\" });
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
