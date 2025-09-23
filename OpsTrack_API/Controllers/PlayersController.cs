using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace OpsTrack_API.Controllers
{
    [ApiController]
    [Route("players")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        //GET
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _playerService.GetAllAsync();
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
