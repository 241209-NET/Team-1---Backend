using PokemonTracker.API.Model;
using PokemonTracker.API.Repository;
using PokemonTracker.API.DTO;
using AutoMapper;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

    public TrainerOutDTO CreateNewTrainer(TrainerInDTO trainerIn)
    {
        if (_trainerRepository.GetTrainerByUsername(trainerIn.Username) is not null)
        {
            throw new Exception("Duplicate Trainer");
        }

        var newTrainer = _trainerRepository.CreateNewTrainer(_mapper.Map<Trainer>(trainerIn));


        return _mapper.Map<TrainerOutDTO>(newTrainer);
    }

    public Trainer Login(string username, string password)
    {
        var trainer = _trainerRepository.GetTrainerByUsername(username);

        if (trainer is null)
        {
            throw new Exception("This trainer doesn't exist");
        }
        else if (trainer.Password != password)
        {
            throw new Exception("The password doesn't match");
        }

        return trainer;
    }

    public TrainerOutDTO UpdateTrainer(UpdateDTO trainer)
    {
        var update = _trainerRepository.GetTrainerById(trainer.Id);

        if (update == null)
        {
            throw new Exception("This trainer doesn't exist!");
        }

        update!.Name = trainer.Name;

        update = _trainerRepository.UpdateTrainer(update);

        return _mapper.Map<TrainerOutDTO>(update);
    }

    public TrainerOutDTO? DeleteTrainerByName(string name)
    {
        var trainer = _trainerRepository.GetTrainerByName(name);

        if (trainer is null)
        {
            throw new Exception("This trainer doesn't exist!");
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
        var trainer = _trainerRepository.GetTrainerByName(name);

        if (trainer is null)
        {
            throw new Exception("This trainer does not exist!");
        }

        return _mapper.Map<TrainerOutDTO>(trainer);
    }

    public Trainer GetTrainerById(int id)
    {
        var trainer = _trainerRepository.GetTrainerById(id);
        var team = GetTeam(trainer!.Name); 

        // Check if the team is empty before accessing it
        if (team.Any())
        {
            trainer.Team = _mapper.Map<List<Pkmn>>(team.ElementAt(0).Team);
        }
        else
        {
            trainer.Team = new List<Pkmn>(); // Set to an empty list if no team is found
        }

        return trainer;
    }
}