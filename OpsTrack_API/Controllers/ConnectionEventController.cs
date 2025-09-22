using Application.Dtos;
using Application.Enums;
using Application.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OpsTrack_API.Controllers
{
    [ApiController]
    [Route("events/connections")]
    public class ConnectionEventsController : ControllerBase
    {
        private readonly IConnectionEventService _service;

        public ConnectionEventsController(IConnectionEventService service)
        {
            _service = service;
        }

        //POST
        [HttpPost("join")]
        public async Task<IActionResult> Join(PlayerEventRequest req)
        {
            var result = await _service.RegisterConnectionEventAsync(
                req.GameIdentity,
                req.Name,
                ConnectionEventType.JOIN
            );
            return Ok(result);
        }

        [HttpPost("leave")]
        public async Task<IActionResult> Leave(PlayerEventRequest req)
        {
            var result = await _service.RegisterConnectionEventAsync(
                req.GameIdentity,
                req.Name,
                ConnectionEventType.LEAVE
            );
            return Ok(result);
        }

        //GET
        [HttpGet("log")]
        public async Task<IActionResult> GetLog()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }
    }

}
