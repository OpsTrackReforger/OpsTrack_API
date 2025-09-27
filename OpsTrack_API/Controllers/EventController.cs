using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace OpsTrack_API.Controllers
{
    [ApiController]
    [Route("events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            return ev is null ? NotFound() : Ok(ev);
        }

        [HttpGet("type/{eventTypeId}")]
        public async Task<IActionResult> GetByType(int eventTypeId)
        {
            var events = await _eventService.GetByTypeAsync(eventTypeId);
            return Ok(events);
        }
    }

}
