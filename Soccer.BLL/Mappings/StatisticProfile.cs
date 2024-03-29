﻿using AutoMapper;
using Soccer.BLL.DTOs;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.BLL.Mappings
{
    public class StatisticProfile : Profile
    {
        public StatisticProfile()
        {
            CreateMap<StatisticImportDTO, Statistic>()
            .ForMember(
                dest => dest.Team,
                prop => prop.MapFrom(src => src.Team.Id))
            .ForMember(
                dest => dest.League,
                prop => prop.MapFrom(src => src.League.Id));
            
            CreateMap<Statistic, StatisticVM> ().ReverseMap();
        }
    }
}
