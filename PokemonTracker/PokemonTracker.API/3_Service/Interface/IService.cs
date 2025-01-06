using PokemonTracker.API.Model;

namespace PokemonTracker.API.Service;

public interface IPokemonService
{
    Pkmn? CreateNewPkmn(Pkmn pkmn);
    IEnumerable<Pkmn> GetAllPkmn();
    IEnumerable<Pkmn> GetAllPkmnByType(string type);
    IEnumerable<Pkmn> GetAllPkmnBySpecies(string species);
    Pkmn GetPkmnByName(string name);
    Pkmn? DeletePkmnByName(string name);
}

public interface ITrainerService
{
    Trainer? CreateNewTrainer(Trainer trainer);
    IEnumerable<Trainer> GetAllTrainers();
    Trainer? GetTrainerByName(string name);
    IEnumerable<Trainer> GetTeam(string name);
    Trainer? DeleteTrainerByName(string name);
}