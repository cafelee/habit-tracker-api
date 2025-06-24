using Microsoft.AspNetCore.Mvc;
using HabitTracker.API.DTOs;
using HabitTracker.API.Repositories;
using HabitTracker.API.Services;
using HabitTracker.API.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitController : ControllerBase
    {
        private readonly HabitRepository _repo;
        private readonly AuditService _auditService;

        public HabitController(HabitRepository repo, AuditService auditService)
        {
            _repo = repo;
            _auditService = auditService;
        }

        [HttpPost]
        public async Task<ActionResult<StandardResponse<int>>> CreateHabit([FromBody] HabitCreateDTO dto)
        {
            var id = await _repo.CreateHabitAsync(dto);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            int? userId = dto.UserId;

            await _auditService.LogAsync(userId, "Create", "Habit", id, ipAddress, $"�إ߲ߺD�G{dto.Title}");

            return Ok(new StandardResponse<int>
            {
                Data = id,
                Message = "�ߺD�إߦ��\"
            });
        }

        [HttpGet]
        public async Task<ActionResult<StandardResponse<IEnumerable<HabitCreateDTO>>>> GetAllHabits()
        {
            var habits = await _repo.GetAllHabitsAsync();

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Read", "Habit", null, ipAddress, "���o�����ߺD");

            return Ok(new StandardResponse<IEnumerable<HabitCreateDTO>> { Data = habits });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StandardResponse<HabitCreateDTO>>> GetHabitById(int id)
        {
            var habit = await _repo.GetHabitByIdAsync(id);
            if (habit == null)
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                await _auditService.LogAsync(null, "ReadFailed", "Habit", id, ipAddress, $"�䤣��ߺD�AID: {id}");

                return NotFound(new StandardResponse<HabitCreateDTO> { Success = false, Message = "�䤣����" });
            }

            var ipAddressSuccess = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Read", "Habit", id, ipAddressSuccess, $"���o�ߺD�AID: {id}");

            return Ok(new StandardResponse<HabitCreateDTO> { Data = habit });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StandardResponse<string>>> UpdateHabit(int id, [FromBody] HabitUpdateDTO dto)
        {
            await _repo.UpdateHabitAsync(id, dto);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Update", "Habit", id, ipAddress, $"��s�ߺD�AID: {id}");

            return Ok(new StandardResponse<string> { Message = "��s���\" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<StandardResponse<string>>> DeleteHabit(int id)
        {
            await _repo.DeleteHabitAsync(id);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Delete", "Habit", id, ipAddress, $"�R���ߺD�AID: {id}");

            return Ok(new StandardResponse<string> { Message = "�R�����\" });
        }

        [HttpPost("{id}/track")]
        public async Task<ActionResult<StandardResponse<string>>> TrackHabit(int id, [FromBody] HabitTrackDTO dto)
        {
            var success = await _repo.TrackHabitAsync(id, dto);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (!success)
            {
                await _auditService.LogAsync(null, "TrackFailed", "HabitTrack", id, ipAddress, $"���d���ѡA�ߺDID: {id}");
                return BadRequest(new StandardResponse<string> { Success = false, Message = "���d����" });
            }

            await _auditService.LogAsync(null, "Track", "HabitTrack", id, ipAddress, $"���\���d�A�ߺDID: {id}");
            return Ok(new StandardResponse<string> { Message = "���d���\" });
        }

        [HttpGet("{id}/tracks")]
        public async Task<ActionResult<StandardResponse<IEnumerable<HabitTrackRecordDTO>>>> GetTracks(
            int id, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var tracks = await _repo.GetHabitTracksAsync(id, start, end);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Read", "HabitTrack", id, ipAddress, $"�d�ߥ��d�O���A�ߺDID: {id}");

            return Ok(new StandardResponse<IEnumerable<HabitTrackRecordDTO>>
            {
                Data = tracks
            });
        }
    }
}
