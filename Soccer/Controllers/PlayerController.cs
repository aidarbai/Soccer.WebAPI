using MediatR;
using Microsoft.AspNetCore.Mvc;
using Soccer.BLL.MediatR.Notfications;
using Soccer.BLL.MediatR.Queries.Players;
using Soccer.BLL.Services.Interfaces;
using Soccer.COMMON.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Soccer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IImportService _importService;
        private readonly IPlayerService _playerService;
        private readonly IMediator _mediator;
        
        public PlayerController(
            IImportService importService,
            IPlayerService playerService,
            IMediator mediator)
        {
            _importService = importService;
            _playerService = playerService;
            _mediator = mediator;
        }


        //[HttpGet("ImportAllPlayersByTeamsList")]
        //public async Task<ActionResult> ImportAllPlayersByTeamsListAsync()
        //{
        //    await _importService.ImportAllPlayersByTeamsListAsync();

        //    return Ok();
        //}

        //[HttpGet("ImportPlayersByTeamId/{teamId:maxlength(10)}")]
        //public async Task<ActionResult> ImportPlayersByTeamIdAsync(string teamId, CancellationToken cancellationToken)
        //{
        //    await _mediator.Publish(new ImportPlayersByTeamIdNotification(teamId), cancellationToken);

        //    return Ok();
        //}

        [HttpGet("{id:maxlength(10)}")]
        [SwaggerOperation("Get player by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PlayerVm>> GetPlayerByIdAsync(string id, CancellationToken cancellationToken)
        {
            var player = await _mediator.Send(new GetPlayerByIdQuery(id), cancellationToken);

            return player != null ? Ok(player) : NoContent();
        }
                
        [HttpGet("searchByParameters")]
        [SwaggerOperation("Search players by parameters or get all players paginated")]
        public async Task<PaginatedResponse<PlayerVm>> SearchByParametersAsync([FromQuery] PlayerSearchByParametersModel searchModel, CancellationToken cancellationToken)
        {
            //return await _playerService.SearchByParametersAsync(searchModel);
            
            return await _mediator.Send(new GetPlayersQuery(searchModel), cancellationToken);
        }
    }
}
