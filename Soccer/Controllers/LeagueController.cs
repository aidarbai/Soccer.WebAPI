using MediatR;
using Microsoft.AspNetCore.Mvc;
using Soccer.BLL.MediatR.Queries.Leagues;
using Soccer.BLL.MediatR.Queries.Teams;
using Soccer.BLL.Services.Interfaces;
using Soccer.COMMON.ViewModels;
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
        private readonly IMediator _mediator;
        public LeagueController(
            IImportService importService,
            ILeagueService leagueService,
            IMediator mediator)
        {
            _importService = importService;
            _leagueService = leagueService;
            _mediator = mediator;
        }

        //[HttpGet("ImportLeagueById")]
        //public async Task<ActionResult> ImportLeagueAsync()
        //{
        //    await _importService.ImportLeagueAsync();

        //    return Ok();
        //}

        [HttpGet("{id}")]
        [SwaggerOperation("Get league by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<LeagueVm>> GetLeagueByIdAsync(string id, CancellationToken cancellationToken)
        {
            var team = await _mediator.Send(new GetLeagueByIdQuery(id), cancellationToken);

            return team != null ? Ok(team) : NoContent();
        }

        [HttpGet("searchByParameters")]
        [SwaggerOperation("Search leagues by parameters or get all leagues paginated")]
        public async Task<PaginatedResponse<LeagueVm>> SearchByParametersAsync([FromQuery] LeagueSearchModel searchModel, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetLeaguesQuery(searchModel), cancellationToken);
        }
    }
}
