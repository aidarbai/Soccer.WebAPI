using MediatR;
using Microsoft.AspNetCore.Mvc;
using Soccer.BLL.MediatR.Queries.Teams;
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
        private readonly IMediator _mediator;
        public TeamController(
            IImportService importService,
            IMediator mediator)
        {
            _importService = importService;
            _mediator = mediator;
        }

        //[HttpGet("ImportTeamsByLeague")]
        //public async Task<ActionResult> ImportTeamsAsync()
        //{
        //    await _importService.ImportTeamsByLeagueAsync();

        //    return Ok();
        //}

        [HttpGet("{id}")]
        [SwaggerOperation("Get team by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<TeamVm>> GetTeamByIdAsync(string id, CancellationToken cancellationToken)
        {
            var team = await _mediator.Send(new GetTeamByIdQuery(id), cancellationToken);

            return team != null ? Ok(team) : NoContent();
        }

        [HttpGet("searchByParameters")]
        [SwaggerOperation("Search teams by parameters or get all teams paginated")]
        public async Task<PaginatedResponse<TeamVm>> SearchByParametersAsync([FromQuery] TeamSearchModel searchModel, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetTeamsQuery(searchModel), cancellationToken);
        }
    }
}
