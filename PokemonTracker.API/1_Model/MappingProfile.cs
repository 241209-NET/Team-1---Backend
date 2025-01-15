using AutoMapper;
using Newtonsoft.Json.Linq;
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
        CreateMap<JObject, TrainerInDTO>().ReverseMap();
        CreateMap<UpdateDTO, PkmnInDTO>().ReverseMap();
        CreateMap<UpdateDTO, PkmnOutDTO>().ReverseMap();
        CreateMap<UpdateDTO, Pkmn>().ReverseMap();
        CreateMap<UpdateDTO, Trainer>().ReverseMap();
        CreateMap<UpdateDTO, TrainerInDTO>().ReverseMap();
        CreateMap<UpdateDTO, TrainerOutDTO>().ReverseMap();

    }
}