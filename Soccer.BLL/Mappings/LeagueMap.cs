using AutoMapper;
using Soccer.BLL.DTOs;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.BLL.Mappings
{
    public class LeagueMap : Profile
    {
        public LeagueMap()
        {
            CreateMap<ResponseLeagueImportDTO, League>()
            .ForMember(dest => dest.Id, prop => prop.MapFrom(src => src.League.Id.ToString()))
            .ForMember(dest => dest.Name, prop => prop.MapFrom(src => src.League.Name))
            .ForMember(dest => dest.Logo, prop => prop.MapFrom(src => src.League.Logo))
            .ForMember(dest => dest.Country, prop => prop.MapFrom(src => src.Country.Name))
            .ForMember(dest => dest.Flag, prop => prop.MapFrom(src => src.Country.Flag));

            CreateMap<LeagueVm, League>().ReverseMap();
        }
    }
}
