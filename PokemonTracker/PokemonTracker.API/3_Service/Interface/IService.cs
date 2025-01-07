using PokemonTracker.API.DTO;
using PokemonTracker.API.Model;

namespace PokemonTracker.API.Service;

public interface IPokemonService
{
    Pkmn? CreateNewPkmn(Pkmn pkmn);
    IEnumerable<PkmnOutDTO> GetAllPkmn();
    IEnumerable<PkmnOutDTO> GetAllPkmnByType(string type);
    IEnumerable<PkmnOutDTO> GetAllPkmnBySpecies(string species);
    PkmnOutDTO GetPkmnByName(string name);
    PkmnOutDTO? DeletePkmnByName(string name);
}

public interface ITrainerService
{
    Trainer? CreateNewTrainer(Trainer trainer);
    IEnumerable<TrainerOutDTO> GetAllTrainers();
    Trainer? GetTrainerByName(string name);
    IEnumerable<Trainer> GetTeam(string name);
    Trainer? DeleteTrainerByName(string name);
}