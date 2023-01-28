using MediatR;
using Microsoft.Extensions.Logging;
using Soccer.BLL.MediatR.Notfications;

namespace Soccer.BLL.MediatR.Handlers
{
    public class SmsHandler : INotificationHandler<ImportPlayersByTeamIdNotification>
    {
        private readonly ILogger<SmsHandler> _logger;

        public SmsHandler(ILogger<SmsHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(ImportPlayersByTeamIdNotification notification, CancellationToken cancellationToken)
        {
           await Task.Run(() => _logger.LogInformation("Importing players for team {team}, sms handler called", notification.Team), cancellationToken);
        }
    }
}
