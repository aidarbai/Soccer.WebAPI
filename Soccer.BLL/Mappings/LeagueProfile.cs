using Soccer.COMMON.ViewModels;

namespace Soccer.BLL.Mappings
{
    public class LeagueProfile : Profile
    {
        public LeagueProfile()
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
