using MediatR;

namespace Soccer.BLL.MediatR.Notfications
{
    public record PlayersAddedNotification(string Team) : INotification;
    
}
