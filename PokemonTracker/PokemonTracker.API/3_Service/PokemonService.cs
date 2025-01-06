namespace PokemonTracker.API.Service;

using System.Collections.Generic;
using Newtonsoft.Json;
using PokeApiNet;
using PokemonTracker.API.Model;
using PokemonTracker.API.Repository;

public class PokemonService : IPokemonService
{
    private readonly IPokemonRepository _pokemonRepository;

    private static readonly HttpClient pokeApi = new()
    {
        BaseAddress = new Uri("https://pokeapi.co/api/v2/")
    };


    public PokemonService(IPokemonRepository pokemonRepository) => _pokemonRepository = pokemonRepository;

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

    public IEnumerable<Pkmn> GetAllPkmn()
    {
        return _pokemonRepository.GetAllPkmn();
    }

    public IEnumerable<Pkmn> GetAllPkmnBySpecies(string species)
    {
        return _pokemonRepository.GetAllPkmnBySpecies(species);
    }

    public IEnumerable<Pkmn> GetAllPkmnByType(string type)
    {
        type = type.ToLower();
        return _pokemonRepository.GetAllPkmnByType(type);
    }

    public Pkmn GetPkmnByName(string name)
    {
        return _pokemonRepository.GetPkmnByName(name);
    }
}