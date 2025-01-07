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
    private readonly IMapper _mapper;

    private static readonly HttpClient pokeApi = new()
    {
        BaseAddress = new Uri("https://pokeapi.co/api/v2/")
    };


    public PokemonService(IPokemonRepository pokemonRepository, IMapper mapper) 
    {
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    } 

    public PkmnOutDTO? CreateNewPkmn(PkmnInDTO newPkmn)
    {
        int trainerID = newPkmn.TrainerID;

        //var trainer = GetTrainerById(trainerID);

        var actualPkmn = pokeApi.GetAsync($"pokemon/{newPkmn.Species.ToLower()}").Result;
        Pokemon pokemonJSON = JsonConvert.DeserializeObject<Pokemon>(actualPkmn.Content.ReadAsStringAsync().Result)!;

        if (actualPkmn is null || GetPkmnByName(newPkmn.Name) is not null)
        {
            return null;
        }

        Pkmn pkmn = _mapper.Map<Pkmn>(newPkmn);

        pkmn.Species = pokemonJSON!.Species.Name;

        pkmn.Type = "";

        foreach (PokemonType t in pokemonJSON.Types)
        {
            pkmn.Type += t.Type.Name + " ";
        }

        pkmn.Type = pkmn.Type.Trim().Replace(" ", "/");
        
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
        return _mapper.Map<PkmnOutDTO>(deletedPkmn);
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