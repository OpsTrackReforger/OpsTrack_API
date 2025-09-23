using Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace OpsTrack_API.Controllers
{
    [ApiController]
    [Route("players")]
    public class PlayersController : ControllerBase
    {
        private readonly IConnectionEventService _eventService;
        private readonly IPlayerService _playerService;

        public PlayersController(IConnectionEventService eventService, IPlayerService playerService)
        {
            _eventService = eventService;
            _playerService = playerService;
        }

        //GET
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _eventService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("Online")]
        public async Task<IActionResult> GetOnline()
        {
            var result = await _playerService.GetOnline();
            return Ok(result);
        }
    }
}
