using AutoMapper;
using Newtonsoft.Json;
using PokeApiNet;
using PokemonTracker.API.DTO;
using PokemonTracker.API.Model;
using PokemonTracker.API.Repository;

namespace PokemonTracker.API.Service;

public class PokemonService : IPokemonService
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly ITrainerService _trainerService;
    private readonly IMapper _mapper;

    public PokemonService(IPokemonRepository pokemonRepository, IMapper mapper, ITrainerService trainerService) 
    {
        _pokemonRepository = pokemonRepository;
        _trainerService = trainerService;
        _mapper = mapper;
    } 

    public PkmnOutDTO? CreateNewPkmn(PkmnInDTO newPkmn)
    {
        int trainerID = newPkmn.TrainerID;

        var trainer = _trainerService.GetTrainerById(trainerID);


        if (GetPkmnByName(newPkmn.Name) is not null || trainer.Team.Count >= 6)
        {
            return null;
        }

        Pkmn pkmn = _mapper.Map<Pkmn>(newPkmn);

        pkmn.Species = newPkmn.Species;

        pkmn.Type = newPkmn.Type;

        pkmn.Type = pkmn.Type.Trim().Replace(" ", "/");

        pkmn.PokedexDesc = newPkmn.PokedexDesc;
        
        return _mapper.Map<PkmnOutDTO>(_pokemonRepository.CreateNewPkmn(pkmn));
    }

    public PkmnOutDTO? DeletePkmnByName(string name)
    {
        var pkmn = _pokemonRepository.GetPkmnByName(name);

        if (pkmn is null)
        {
            return null;
        }

        var deletedPkmn = _pokemonRepository.DeletePkmnByName(pkmn);
        return _mapper.Map<PkmnOutDTO>(pkmn);
    }

    public IEnumerable<PkmnOutDTO> GetAllPkmn()
    {
        var pkmnList = _pokemonRepository.GetAllPkmn();
        return _mapper.Map<List<PkmnOutDTO>>(pkmnList);
    }

    public IEnumerable<PkmnOutDTO> GetAllPkmnBySpecies(string species)
    {
        var speciesList = _pokemonRepository.GetAllPkmnBySpecies(species);
        return _mapper.Map<List<PkmnOutDTO>>(speciesList);
    }

    public IEnumerable<PkmnOutDTO> GetAllPkmnByType(string type)
    {
        type = type.ToLower();
        var typeList = _pokemonRepository.GetAllPkmnByType(type);
        return _mapper.Map<List<PkmnOutDTO>>(typeList);
    }

    public PkmnOutDTO? GetPkmnByName(string name)
    {
        var pkmn = _pokemonRepository.GetPkmnByName(name);
        return _mapper.Map<PkmnOutDTO>(pkmn);
    }
}