using System.Collections.Immutable;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        Pkmn jsonPkmn =  _mapper.Map<Pkmn>(newPkmn);
        int trainerID = jsonPkmn.TrainerID;

        var trainer = _trainerService.GetTrainerById(trainerID);

        if (trainer!.Team.Count >= 6)
        {
            throw new Exception("This trainer's team is already full! Please remove a pokemon first.");
        }
        
        return _mapper.Map<PkmnOutDTO>(_pokemonRepository.CreateNewPkmn(jsonPkmn));
    }

    public PkmnOutDTO UpdatePkmn(UpdateDTO pkmn)
    {
        var update = _pokemonRepository.GetPkmnById(pkmn.Id);

        update.Name = pkmn.Name;

        update = _pokemonRepository.UpdatePkmn(update);

        return _mapper.Map<PkmnOutDTO>(update);
    }

    public PkmnOutDTO? DeletePkmn(int id)
    {
        var deletedPkmn = _pokemonRepository.DeletePkmn(_pokemonRepository.GetPkmnById(id));
        return _mapper.Map<PkmnOutDTO>(deletedPkmn);
    }

    public IEnumerable<PkmnOutDTO> GetAllPkmn()
    {
        var pkmnList = _pokemonRepository.GetAllPkmn();
        return _mapper.Map<List<PkmnOutDTO>>(pkmnList);
    }
}