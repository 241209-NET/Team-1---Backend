using Microsoft.EntityFrameworkCore;
using PokemonTracker.API.Data;
using PokemonTracker.API.Model;

namespace PokemonTracker.API.Repository;

public class TrainerRepository : ITrainerRepository
{
    private readonly PokemonContext _trainerContext;

    public TrainerRepository(PokemonContext trainerContext) => _trainerContext = trainerContext;
 

    public Trainer CreateNewTrainer(Trainer trainer)
    {
        _trainerContext.Trainers.Add(trainer);
        _trainerContext.SaveChanges();
        return trainer;
    }

    public Trainer? DeleteTrainerByName(Trainer trainer)
    {
        _trainerContext.Trainers.Remove(trainer);
        _trainerContext.SaveChanges();

        return trainer;
    }

    public IEnumerable<Trainer> GetAllTrainers()
    {
        return _trainerContext.Trainers.ToList();
    }

    public IEnumerable<Trainer> GetTeam(string name)
    {
        var pkmnTeam = _trainerContext.Trainers
            .Include(t => t.Team)
            .Where(t => t.Name == name);

        return pkmnTeam;
    }

    public Trainer? GetTrainerByName(string name)
    {
        var trainer = _trainerContext.Trainers.Where(t => t.Name == name).First();

        return trainer;
    }
}