﻿using MediatR;
using Soccer.DAL.Models;

namespace Soccer.BLL.MediatR.Queries
{
    public record GetTeamByIdQuery(string id) : IRequest<Team>;
}
