using Newtonsoft.Json.Linq;
using PokemonTracker.API.DTO;
using PokemonTracker.API.Model;

namespace PokemonTracker.API.Service;

public interface IPokemonService
{
    PkmnOutDTO? CreateNewPkmn(PkmnInDTO pkmn);
    IEnumerable<PkmnOutDTO> GetAllPkmn();
    PkmnOutDTO? DeletePkmn(int id);
    PkmnOutDTO UpdatePkmn(UpdateDTO toUpdate);
}

public interface ITrainerService
{
    TrainerOutDTO CreateNewTrainer(TrainerInDTO trainer);
    IEnumerable<TrainerOutDTO> GetAllTrainers();
    TrainerOutDTO? GetTrainerByName(string name);
    IEnumerable<TrainerOutDTO> GetTeam(string name);
    TrainerOutDTO? DeleteTrainerByName(string name);
    Trainer? GetTrainerById(int id);
    int Login(string username, string password);
    TrainerOutDTO UpdateTrainer(UpdateDTO toUpdate);
}