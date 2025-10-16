using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace OpsTrack_API.Controllers
{
    [ApiController]
    [Route("events/combat")]
    public class CombatEventsController : ControllerBase
    {
        private readonly ICombatEventService _combatService;

        public CombatEventsController(ICombatEventService combatService)
        {
            _combatService = combatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _combatService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _combatService.GetByIdAsync(id);
            return ev is null ? NotFound() : Ok(ev);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCombatEvent(CombatEventRequest req)
        {
            var result = await _combatService.RegisterCombatEventAsync(req);
            return Ok(result);
        }


        /// <summary>
        /// Get combat events within a specified date range.
        /// </summary>
        /// <param name="start">
        /// Start date (inclusive) in ISO 8601 format, e.g., 2023-01-01T00:00:00Z
        /// </param>
        /// <param name="end">
        /// End date (inclusive) in ISO 8601 format, e.g., 2023-01-31T23:59:59Z
        /// </param>
        /// <returns></returns>
        [HttpGet("byDate")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var events = await _combatService.GetByDateRangeAsync(start, end);
            return Ok(events);
        }
    }

}
