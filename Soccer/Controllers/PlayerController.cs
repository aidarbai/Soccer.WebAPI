using MediatR;
using Microsoft.AspNetCore.Mvc;
using Soccer.BLL.MediatR.Commands;
using Soccer.BLL.MediatR.Notfications;
using Soccer.BLL.MediatR.Queries;
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


        [HttpGet("ImportAllPlayersByTeamsList")]
        public async Task<ActionResult> ImportAllPlayersByTeamsListAsync()
        {
            await _importService.ImportAllPlayersByTeamsListAsync();

            return Ok();
        }

        [HttpGet("ImportPlayersByTeamId/{teamId}")]
        public async Task<ActionResult> ImportPlayersByTeamIdAsync(string teamId, CancellationToken cancellationToken) //TODO how to validate?
        {
            await _mediator.Send(new ImportPlayersCommand(teamId), cancellationToken); //TODO use just one publish
            await _mediator.Publish(new PlayersAddedNotification(teamId), cancellationToken);

            return Ok();
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get player by ID")]
        public async Task<ActionResult<PlayerVM>> GetPlayerByIdAsync(string id, CancellationToken cancellationToken)
        {
            //var player = await _playerService.GetByIdAsync(id);

            var player = await _mediator.Send(new GetPlayerByIdQuery(id), cancellationToken);

            return player != null ? Ok(player) : NoContent(); //TODO organize handlers folder
        }

        [HttpGet("searchByParameters")]
        [SwaggerOperation("Search players by parameters or get all players paginated")]
        public async Task<PaginatedResponse<PlayerVM>> SearchByParametersAsync([FromQuery] PlayerSearchByParametersModel searchModel, CancellationToken cancellationToken)
        {
            //return await _playerService.SearchByParametersAsync(searchModel);
            
            return await _mediator.Send(new GetPlayersQuery(searchModel), cancellationToken);
        }
    }
}
