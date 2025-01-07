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
    TrainerInDTO CreateNewTrainer(Trainer trainer);
    IEnumerable<TrainerOutDTO> GetAllTrainers();
    TrainerOutDTO? GetTrainerByName(string name);
    IEnumerable<Trainer> GetTeam(string name);
    TrainerOutDTO? DeleteTrainerByName(string name);
}