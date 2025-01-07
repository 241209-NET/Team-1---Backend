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

    public TrainerOutDTO? CreateNewTrainer(TrainerInDTO trainerIn)
    {
        var trainer = _mapper.Map<Trainer>(trainerIn);
        var newTrainer = _trainerRepository.CreateNewTrainer(trainer);

        return _mapper.Map<TrainerOutDTO>(newTrainer);
    }

    public TrainerOutDTO? DeleteTrainerByName(string name)
    {
        var trainer = _trainerRepository.GetTrainerByName(name);

        if (trainer is null)
        {
            return null;
        }

        var deletedTrainer = _trainerRepository.DeleteTrainerByName(trainer);
        return _mapper.Map<TrainerOutDTO>(deletedTrainer);
    }

    public IEnumerable<TrainerOutDTO> GetAllTrainers()
    {
        var trainerList = _trainerRepository.GetAllTrainers();
        return _mapper.Map<List<TrainerOutDTO>>(trainerList);       
    }
 
    public IEnumerable<TrainerOutDTO> GetTeam(string name)
    {
        return _mapper.Map<IEnumerable<TrainerOutDTO>>(_trainerRepository.GetTeam(name));
    }

    public TrainerOutDTO? GetTrainerByName(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;

        var trainer = _trainerRepository.GetTrainerByName(name);
        return _mapper.Map<TrainerOutDTO>(trainer);
    }

    public Trainer? GetTrainerById(int id)
    {
        var trainer = _trainerRepository.GetTrainerById(id);

        var team = GetTeam(trainer.Name);

        trainer.Team = _mapper.Map<List<Pkmn>>(team.ElementAt(0).Team);
    
        return trainer;
    }


}