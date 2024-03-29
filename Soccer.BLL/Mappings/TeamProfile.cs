﻿using AutoMapper;
using Soccer.BLL.DTOs;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.BLL.Mappings
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<ResponseTeamImportDTO, Team>()
            .ForMember(dest => dest.Id, prop => prop.MapFrom(src => src.Team.Id.ToString()))
            .ForMember(dest => dest.Name, prop => prop.MapFrom(src => src.Team.Name))
            .ForMember(dest => dest.Founded, prop => prop.MapFrom(src => src.Team.Founded))
            .ForMember(dest => dest.Logo, prop => prop.MapFrom(src => src.Team.Logo))
            .ForMember(dest => dest.Venue, prop => prop.MapFrom(src => src.Venue.Name))
            .ForMember(dest => dest.City, prop => prop.MapFrom(src => src.Venue.City))
            .ForMember(dest => dest.League, opt => opt.Ignore())
            .ForMember(dest => dest.CardColor, opt => opt.Ignore());

            CreateMap<TeamVm, Team>().ReverseMap();
        }
    }
}
