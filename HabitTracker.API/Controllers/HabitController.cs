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

            await _auditService.LogAsync(userId, "Create", "Habit", id, ipAddress, $"ミ策篋{dto.Title}");

            return Ok(new StandardResponse<int>
            {
                Data = id,
                Message = "策篋ミΘ"
            });
        }

        [HttpGet]
        public async Task<ActionResult<StandardResponse<IEnumerable<HabitCreateDTO>>>> GetAllHabits()
        {
            var habits = await _repo.GetAllHabitsAsync();

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Read", "Habit", null, ipAddress, "眔场策篋");

            return Ok(new StandardResponse<IEnumerable<HabitCreateDTO>> { Data = habits });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StandardResponse<HabitCreateDTO>>> GetHabitById(int id)
        {
            var habit = await _repo.GetHabitByIdAsync(id);
            if (habit == null)
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                await _auditService.LogAsync(null, "ReadFailed", "Habit", id, ipAddress, $"тぃ策篋ID: {id}");

                return NotFound(new StandardResponse<HabitCreateDTO> { Success = false, Message = "тぃ戈" });
            }

            var ipAddressSuccess = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Read", "Habit", id, ipAddressSuccess, $"眔策篋ID: {id}");

            return Ok(new StandardResponse<HabitCreateDTO> { Data = habit });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StandardResponse<string>>> UpdateHabit(int id, [FromBody] HabitUpdateDTO dto)
        {
            await _repo.UpdateHabitAsync(id, dto);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Update", "Habit", id, ipAddress, $"穝策篋ID: {id}");

            return Ok(new StandardResponse<string> { Message = "穝Θ" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<StandardResponse<string>>> DeleteHabit(int id)
        {
            await _repo.DeleteHabitAsync(id);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Delete", "Habit", id, ipAddress, $"埃策篋ID: {id}");

            return Ok(new StandardResponse<string> { Message = "埃Θ" });
        }

        [HttpPost("{id}/track")]
        public async Task<ActionResult<StandardResponse<string>>> TrackHabit(int id, [FromBody] HabitTrackDTO dto)
        {
            var success = await _repo.TrackHabitAsync(id, dto);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (!success)
            {
                await _auditService.LogAsync(null, "TrackFailed", "HabitTrack", id, ipAddress, $"ゴア毖策篋ID: {id}");
                return BadRequest(new StandardResponse<string> { Success = false, Message = "ゴア毖" });
            }

            await _auditService.LogAsync(null, "Track", "HabitTrack", id, ipAddress, $"Θゴ策篋ID: {id}");
            return Ok(new StandardResponse<string> { Message = "ゴΘ" });
        }

        [HttpGet("{id}/tracks")]
        public async Task<ActionResult<StandardResponse<IEnumerable<HabitTrackRecordDTO>>>> GetTracks(
            int id, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var tracks = await _repo.GetHabitTracksAsync(id, start, end);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await _auditService.LogAsync(null, "Read", "HabitTrack", id, ipAddress, $"琩高ゴ癘魁策篋ID: {id}");

            return Ok(new StandardResponse<IEnumerable<HabitTrackRecordDTO>>
            {
                Data = tracks
            });
        }
    }
}
