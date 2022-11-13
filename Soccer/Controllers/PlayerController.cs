using Microsoft.AspNetCore.Mvc;
using Soccer.BLL.Helpers;
using Soccer.BLL.Services;
using Soccer.BLL.Services.Interfaces;
using Soccer.COMMON.ViewModels;
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

            var test = players.Where(p => p.Statistics.Count > 1);

            return test;
        }

        [HttpGet]
        [SwaggerOperation("Get players paginated")]
        public async Task<PaginatedResponse<PlayerVM>> GetPlayersPaginateAsync([FromQuery] SortAndPagePlayerModel model)
        {
            return await _playerService.GetPlayersPaginatedAsync(model);
        }

        [HttpGet("team/{teamId}")]
        [SwaggerOperation("Get all players from team")]
        public async Task<IEnumerable<Player>> GetPlayersByTeamAsync(string teamId)
        {
            var players = await _playerService.GetPlayersByTeamIdAsync(teamId);

            return players;
        }


        [HttpGet("{id}")]
        [SwaggerOperation("Get player by ID")]
        public async Task<ActionResult<Player>> GetLeagueByIdAsync(string id)
        {
            var player = await _playerService.GetByIdAsync(id);

            return player != null ? Ok(player) : NotFound();
        }

        [HttpGet("searchByName")]
        [SwaggerOperation("Find teams by name")]
        public async Task<IEnumerable<Player>> FindTeamsAsync(string player)
        {
            return await _playerService.SearchByNameAsync(player);
        }


        [HttpGet("searchByAge")]
        [SwaggerOperation("Find teams by name")]
        public async Task<PaginatedResponse<PlayerVM>> SearchByAgeAsync(int from, int to, [FromQuery] SortAndPagePlayerModel model)
        {
            return await _playerService.SearchByAgeAsync(from, to, model);
        }
    }
}
