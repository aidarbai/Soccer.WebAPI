using AutoMapper;
using Soccer.BLL.DTOs;
using Soccer.DAL.Models;

namespace Soccer.BLL.Mappings
{
    public class StatisticMap : Profile
    {
        public StatisticMap()
        {
            CreateMap<StatisticDTO, Statistic>()
            .ForMember(
                dest => dest.Team,
                prop => prop.MapFrom(src => src.Team.Id))
            .ForMember(
                dest => dest.League,
                prop => prop.MapFrom(src => src.League.Id));

        }
    }
}
