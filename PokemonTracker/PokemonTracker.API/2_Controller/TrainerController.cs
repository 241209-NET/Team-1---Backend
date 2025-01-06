using Microsoft.AspNetCore.Mvc;
using PokemonTracker.API.Model;
using PokemonTracker.API.Service;

namespace PokemonTracker.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class TrainerController : ControllerBase
{
    private readonly ITrainerService _trainerService;

    public TrainerController(ITrainerService trainerService) => _trainerService = trainerService;

    [HttpPost]
    public IActionResult CreateNewTrainer(Trainer newTrainer)
    {
        var trainer = _trainerService.CreateNewTrainer(newTrainer);

        if (trainer is null)
        {
            return NotFound();
        }

        return Ok(trainer);
    }

    [HttpGet]
    public IActionResult GetAllTrainers()
    {
        var trainerList = _trainerService.GetAllTrainers();
        return Ok(trainerList);
    }

    [HttpGet("team/{name}")]
    public IActionResult GetTeam(string name)
    {
        var team = _trainerService.GetTeam(name);

        return Ok(team);
    }

    [HttpGet("name/{name}")]
    public IActionResult GetTrainerByName(string name)
    {
        var findTrainer = _trainerService.GetTrainerByName(name);

        if (findTrainer is null)
        {
            return NotFound(); 
        }

        return Ok(findTrainer);
    }

    [HttpDelete("delete/{name}")]
    public IActionResult DeleteTrainerByName(string name)
    {
        var deleteTrainer = _trainerService.DeleteTrainerByName(name);

        if (deleteTrainer is null)
        {
            return NotFound();
        }

        return Ok(deleteTrainer);
    }
}