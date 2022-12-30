using MediatR;
using Microsoft.AspNetCore.Mvc;
using Soccer.BLL.MediatR.Queries;
using Soccer.BLL.Services.Interfaces;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Soccer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IImportService _importService;
        private readonly ITeamService _teamService;
        private readonly IMediator _mediator;
        public TeamController(
            IImportService importService,
            ITeamService teamService,
            IMediator mediator)
        {
            _importService = importService;
            _teamService = teamService;
            _mediator = mediator;
        }

        [HttpGet("ImportTeamsByLeague")]
        public async Task<ActionResult> ImportTeamsAsync()
        {
            await _importService.ImportTeamsByLeagueAsync();

            return Ok();
        }

        [HttpGet("getall")]
        [SwaggerOperation("Get all teams")]
        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            var teams = await _mediator.Send(new GetTeamsQuery()); // TODO read about Cancellation token


            //TODO try out global using

            //return await _teamService.GetAllAsync();
            return teams;
        }

        [HttpGet]
        [SwaggerOperation("Get teams paginated")]
        public async Task<PaginatedResponse<TeamVm>> GetTeamsPaginateAsync([FromQuery] SortAndPageTeamModel model)
        {
            return await _teamService.GetTeamsPaginateAsync(model);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get team by ID")]
        public async Task<ActionResult<Team>> GetTeamByIdAsync(string id)
        {
            //var team = await _teamService.GetByIdAsync(id);
            var team = await _mediator.Send(new GetTeamByIdQuery(id)); // TODO try publish

            return team != null ? Ok(team) : NotFound();
        }

        [HttpGet("search")]
        [SwaggerOperation("Find teams by name")]
        public async Task<IEnumerable<Team>> FindTeamsAsync(string team)
        {
            return await _teamService.SearchByNameAsync(team);
        }
    }
}
