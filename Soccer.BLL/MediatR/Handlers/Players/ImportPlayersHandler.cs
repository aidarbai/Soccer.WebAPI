using MediatR;
using Soccer.BLL.MediatR.Notfications;

namespace Soccer.BLL.MediatR.Handlers.Players
{
    public class ImportPlayersHandler : INotificationHandler<ImportPlayersByTeamIdNotification>
    {
        private readonly IImportService _importService;
        private readonly ILogger<ImportPlayersHandler> _logger;

        public ImportPlayersHandler(
            IImportService importService,
            ILogger<ImportPlayersHandler> logger)
        {
            _importService = importService;
            _logger = logger;

        }
        public async Task Handle(ImportPlayersByTeamIdNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Calling import service.");
            await _importService.ImportPlayersByTeamAsync(notification.Team);
        }
    }
}