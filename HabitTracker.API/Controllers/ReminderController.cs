using Microsoft.AspNetCore.Mvc;
using HabitTracker.API.DTOs;
using HabitTracker.API.Repositories;
using HabitTracker.API.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/reminders")]
    public class ReminderController : ControllerBase
    {
        private readonly HabitRepository _repo;

        public ReminderController(HabitRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("priorities")]
        public async Task<ActionResult<StandardResponse<IEnumerable<ReminderPriorityDTO>>>> GetReminderPriorities(
            [FromQuery] int userId,
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var list = await _repo.GetReminderPrioritiesAsync(userId, start, end);
            return Ok(new StandardResponse<IEnumerable<ReminderPriorityDTO>> { Data = list });
        }
    }
}
