using PokemonTracker.API.DTO;
using PokemonTracker.API.Model;

namespace PokemonTracker.API.Service;

public interface IPokemonService
{
    PkmnOutDTO? CreateNewPkmn(PkmnInDTO pkmn);
    IEnumerable<PkmnOutDTO> GetAllPkmn();
    IEnumerable<PkmnOutDTO> GetAllPkmnByType(string type);
    IEnumerable<PkmnOutDTO> GetAllPkmnBySpecies(string species);
    PkmnOutDTO? GetPkmnByName(string name);
    PkmnOutDTO? DeletePkmnByName(string name);
}

public interface ITrainerService
{
    TrainerOutDTO CreateNewTrainer(TrainerInDTO trainer);
    IEnumerable<TrainerOutDTO> GetAllTrainers();
    TrainerOutDTO? GetTrainerByName(string name);
    IEnumerable<TrainerOutDTO> GetTeam(string name);
    TrainerOutDTO? DeleteTrainerByName(string name);
    Trainer? GetTrainerById(int id);
}