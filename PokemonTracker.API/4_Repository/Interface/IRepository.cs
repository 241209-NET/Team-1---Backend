using PokemonTracker.API.Model;

namespace PokemonTracker.API.Repository;

public interface IPokemonRepository
{
    Pkmn CreateNewPkmn(Pkmn pkmn);
    IEnumerable<Pkmn> GetAllPkmn();
    Pkmn? DeletePkmn(Pkmn pkmn);
    Pkmn GetPkmnById(int id);
    Pkmn UpdatePkmn(Pkmn toUpdate);

}

public interface ITrainerRepository
{
    Trainer? CreateNewTrainer(Trainer trainer);
    IEnumerable<Trainer> GetAllTrainers();
    Trainer? GetTrainerByName(string name);
    IEnumerable<Trainer> GetTeam(string name);
    Trainer? DeleteTrainerByName(Trainer trainer);
    Trainer? GetTrainerById(int id);
    Trainer GetTrainerByUsername(string username);
    Trainer UpdateTrainer(Trainer trainer);
}