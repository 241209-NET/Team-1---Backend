namespace PokemonTracker.API.Service;

using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using PokeApiNet;
using PokemonTracker.API.DTO;
using PokemonTracker.API.Model;
using PokemonTracker.API.Repository;

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

    public Pkmn? CreateNewPkmn(Pkmn newPkmn)
    {
        var actualPkmn = pokeApi.GetAsync($"pokemon/{newPkmn.Species.ToLower()}").Result;
        Pokemon pokemonJSON = JsonConvert.DeserializeObject<Pokemon>(actualPkmn.Content.ReadAsStringAsync().Result)!;

        if (actualPkmn is null || GetPkmnByName(newPkmn.Name) is not null)
        {
            return null;
        }

        newPkmn.Species = pokemonJSON!.Species.Name;

        newPkmn.Type = "";

        foreach (PokemonType t in pokemonJSON.Types)
        {
            newPkmn.Type += t.Type.Name + "/";
        }
        
        return _pokemonRepository.CreateNewPkmn(newPkmn);
    }

    public Pkmn? DeletePkmnByName(string name)
    {
        var pkmn = GetPkmnByName(name);

        if (pkmn is null)
        {
            return null;
        }
        
        return _pokemonRepository.DeletePkmnByName(pkmn);;
    }

    public IEnumerable<PkmnOutDTO> GetAllPkmn()         // ✅
    {
        var pkmnList = _pokemonRepository.GetAllPkmn();
        List<PkmnOutDTO> pkmnDTOList = _mapper.Map<List<PkmnOutDTO>>(pkmnList);

        return pkmnDTOList; 
    }

    public IEnumerable<PkmnOutDTO> GetAllPkmnBySpecies(string species)     // ✅
    {
        var speciesList = _pokemonRepository.GetAllPkmnBySpecies(species);
        List<PkmnOutDTO> pkmnDTOList = _mapper.Map<List<PkmnOutDTO>>(speciesList);

        return pkmnDTOList;
    }

    public IEnumerable<PkmnOutDTO> GetAllPkmnByType(string type)        // ✅
    {
        type = type.ToLower();
        var typeList = _pokemonRepository.GetAllPkmnByType(type);
        List<PkmnOutDTO> pkmnDTOList = _mapper.Map<List<PkmnOutDTO>>(typeList);

        return pkmnDTOList;
    }

    public PkmnOutDTO GetPkmnByName(string name)        // ✅
    {
        var pkmn = _pokemonRepository.GetPkmnByName(name);
        PkmnOutDTO pkmnDTO = _mapper.Map<PkmnOutDTO>(pkmn);

        return pkmnDTO;
    }
}