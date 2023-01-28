using MediatR;

namespace Soccer.BLL.MediatR.Notfications
{
    public record ImportPlayersByTeamIdNotification(string Team) : INotification;
    
}
