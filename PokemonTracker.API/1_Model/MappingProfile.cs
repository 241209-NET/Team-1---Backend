using AutoMapper;
using PokemonTracker.API.Model;

namespace PokemonTracker.API.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Pkmn, PkmnInDTO>().ReverseMap();
        CreateMap<Pkmn, PkmnOutDTO>().ReverseMap();
        CreateMap<Trainer, TrainerInDTO>().ReverseMap();
        CreateMap<Trainer, TrainerOutDTO>().ReverseMap();
    }
}