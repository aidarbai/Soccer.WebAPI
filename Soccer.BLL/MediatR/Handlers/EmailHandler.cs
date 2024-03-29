﻿using MediatR;
using Microsoft.Extensions.Logging;
using Soccer.BLL.MediatR.Notfications;

namespace Soccer.BLL.MediatR.Handlers
{
    public class EmailHandler : INotificationHandler<ImportPlayersByTeamIdNotification>
    {
        private readonly ILogger<EmailHandler> _logger;

        public EmailHandler(ILogger<EmailHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(ImportPlayersByTeamIdNotification notification, CancellationToken cancellationToken)
        {
           await Task.Run(() => _logger.LogInformation("Importing players for team {team}, email handler called", notification.Team), cancellationToken);
        }
    }
}
