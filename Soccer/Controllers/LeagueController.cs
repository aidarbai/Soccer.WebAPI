using Microsoft.AspNetCore.Mvc;
using Soccer.BLL.Services.Interfaces;
using Soccer.DAL.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Soccer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly IImportService _importService;
        private readonly ILeagueService _leagueService;
        public LeagueController(
            IImportService importService,
            ILeagueService leagueService)
        {
            _importService = importService;
            _leagueService = leagueService;
        }

        [HttpGet("ImportLeagueById")]
        public async Task<ActionResult> ImportLeagueAsync()
        {
            await _importService.ImportLeagueAsync();

            return Ok();
        }

        [HttpGet]
        [SwaggerOperation("Get all leagues")]
        public async Task<IEnumerable<League>> GetLeaguesAsync()
        {
            return await _leagueService.GetAllAsync();
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get league by ID")]
        public async Task<ActionResult<League>> GetLeagueByIdAsync(string id)
        {
            var league = await _leagueService.GetByIdAsync(id);

            return league != null ? Ok(league) : NotFound();
        }
    }
}
