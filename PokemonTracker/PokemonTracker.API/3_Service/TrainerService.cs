using PokemonTracker.API.Model;
using PokemonTracker.API.Repository;

namespace PokemonTracker.API.Service;

public class TrainerService : ITrainerService
{
    private readonly ITrainerRepository _trainerRepository;

    public TrainerService(ITrainerRepository trainerRepository) => _trainerRepository = trainerRepository;


    public Trainer? CreateNewTrainer(Trainer trainer)
    {
        return _trainerRepository.CreateNewTrainer(trainer);
    }

    public Trainer? DeleteTrainerByName(string name)
    {
        var trainer = GetTrainerByName(name);

        if (trainer is null)
        {
            return null;
        }

        return _trainerRepository.DeleteTrainerByName(trainer);
    }

    public IEnumerable<Trainer> GetAllTrainers()
    {
        return _trainerRepository.GetAllTrainers();
    }

    public IEnumerable<Trainer> GetTeam(string name)
    {
        return _trainerRepository.GetTeam(name);
    }

    public Trainer? GetTrainerByName(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;

        var trainer = _trainerRepository.GetTrainerByName(name);
        return trainer;
    }
}