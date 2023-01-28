using AutoMapper;
using Soccer.BLL.DTOs;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.BLL.Mappings
{
    public class PlayerMap : Profile
    {
        public PlayerMap()
        {
            CreateMap<PlayerVM, Player>().ReverseMap();

            CreateMap<ResponsePlayerImportDTO, Player>()
            .ForMember(dest => dest.Id, prop => prop.MapFrom(src => src.Player.Id.ToString()))
            .ForMember(dest => dest.Name, prop => prop.MapFrom(src => src.Player.Name))
            .ForMember(dest => dest.Firstname, prop => prop.MapFrom(src => src.Player.Firstname))
            .ForMember(dest => dest.Lastname, prop => prop.MapFrom(src => src.Player.Lastname))
            .ForMember(dest => dest.Birth, prop => prop.MapFrom(src => src.Player.Birth))
            .ForMember(dest => dest.Nationality, prop => prop.MapFrom(src => src.Player.Nationality))
            .ForMember(dest => dest.Height, prop => prop.MapFrom(src => src.Player.Height))
            .ForMember(dest => dest.Weight, prop => prop.MapFrom(src => src.Player.Weight))
            .ForMember(dest => dest.Injured, prop => prop.MapFrom(src => src.Player.Injured))
            .ForMember(dest => dest.Photo, prop => prop.MapFrom(src => src.Player.Photo))
            .ForMember(dest => dest.Statistics, prop => prop.MapFrom(src => src.Statistics));
        }
    }
}
