using PokemonTracker.API.Model;
using PokemonTracker.API.Repository;
using PokemonTracker.API.DTO;
using AutoMapper;

namespace PokemonTracker.API.Service;

public class TrainerService : ITrainerService
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IMapper _mapper;

    public TrainerService(ITrainerRepository trainerRepository, IMapper mapper) 
    {
        _trainerRepository = trainerRepository;
        _mapper = mapper;
    }


    public Trainer? CreateNewTrainer(Trainer trainer)
    {
        return _trainerRepository.CreateNewTrainer(trainer);
    }

    public TrainerOutDTO? DeleteTrainerByName(string name)          // ✅
    {
        var trainer = _trainerRepository.GetTrainerByName(name);

        if (trainer is null)
        {
            return null;
        }

        var deletedTrainer = _trainerRepository.DeleteTrainerByName(trainer);
        return _mapper.Map<TrainerOutDTO>(deletedTrainer);
    }

    public IEnumerable<TrainerOutDTO> GetAllTrainers()              // ✅
    {
        var trainerList = _trainerRepository.GetAllTrainers();
        return _mapper.Map<List<TrainerOutDTO>>(trainerList);       
    }
 
    public IEnumerable<Trainer> GetTeam(string name)
    {
        return _trainerRepository.GetTeam(name);
    }

    public TrainerOutDTO? GetTrainerByName(string name)             // ✅
    {
        if (string.IsNullOrEmpty(name)) return null;

        var trainer = _trainerRepository.GetTrainerByName(name);
        return _mapper.Map<TrainerOutDTO>(trainer);
    }
}