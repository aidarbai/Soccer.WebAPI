using Microsoft.AspNetCore.Mvc;
using Soccer.BLL.Helpers;
using Soccer.BLL.Services.Interfaces;
using Soccer.DAL.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Soccer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IImportService _importService;
        private readonly IPlayerService _playerService;
        private readonly ILogger<PlayerController> _logger;
        public PlayerController(
            IImportService importService,
            IPlayerService playerService,
            ILogger<PlayerController> logger)
        {
            _importService = importService;
            _playerService = playerService;
            _logger = logger;
        }


        [HttpGet("ImportAllPlayersByTeamsList")]
        public async Task<ActionResult> ImportAllPlayersByTeamsListAsync()
        {
            await _importService.ImportAllPlayersByTeamsListAsync();

            return Ok();
        }

        [HttpGet("getall")]
        [SwaggerOperation("Get all players")]
        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {

            var players = await _playerService.GetAllAsync();

            //var test = players.Where(p => p.Statistics.Count > 1 && p.Statistics[0].Team == p.Statistics[1].Team);

            var test = players.Where(p => p.Statistics.Count > 1);

            return test;
        }

        [HttpGet("team/{teamId}")]
        [SwaggerOperation("Get all players from team")]
        public async Task<IEnumerable<Player>> GetPlayersByTeamAsync(string teamId)
        {
            var players = await _playerService.GetByTeamIdAsync(teamId);

            return players;
        }


        [HttpGet("{id}")]
        [SwaggerOperation("Get player by ID")]
        public async Task<ActionResult<Player>> GetLeagueByIdAsync(string id)
        {
            var league = await _playerService.GetByIdAsync(id);

            return league != null ? Ok(league) : NotFound();
        }
    }
}
