using Microsoft.AspNetCore.Mvc;
using PokemonTracker.API.DTO;
using PokemonTracker.API.Service;

namespace PokemonTracker.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class TrainerController : ControllerBase
{
    private readonly ITrainerService _trainerService;

    public TrainerController(ITrainerService trainerService) => _trainerService = trainerService;

    [HttpPost]
    public IActionResult CreateNewTrainer(TrainerInDTO newTrainer)
    {
        try
        {
            var trainer = _trainerService.CreateNewTrainer(newTrainer);
            return Ok(trainer);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("login")]
    public IActionResult Login([FromBody] string username, [FromBody] string password)
    {
        int trainerId = -5;

        try
        {
            trainerId = _trainerService.Login(username, password);
            return Ok(trainerId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
        try
        {
            var findTrainer = _trainerService.GetTrainerByName(name);
            return Ok(findTrainer);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete/{name}")]
    public IActionResult DeleteTrainerByName(string name)
    {
        try
        {
            var deleteTrainer = _trainerService.DeleteTrainerByName(name);
            return Ok(deleteTrainer);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}