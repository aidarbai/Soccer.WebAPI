﻿using MediatR;
using Soccer.BLL.MediatR.Commands;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers
{
    public class AddTeamHandler : IRequestHandler<AddTeamCommand, Team>
    {
        private readonly ITeamRepository _repository;

        public AddTeamHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<Team> Handle(AddTeamCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(request.Team);

            return request.Team;
        }
    }
}
