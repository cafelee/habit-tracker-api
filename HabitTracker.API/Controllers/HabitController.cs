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
    }
}
