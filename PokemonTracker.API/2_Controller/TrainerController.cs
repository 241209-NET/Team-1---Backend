using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PokemonTracker.API.DTO;
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
    public IActionResult CreateNewTrainer([FromBody] TrainerInDTO newTrainer)
    {
        try
        {
            var trainer = _trainerService.CreateNewTrainer(newTrainer);
            return Ok(trainer);
        }
        catch (Exception ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO login)
    {

        try
        {
            Trainer trainer = _trainerService.Login(login.Username, login.Password);
            trainer.Password = "";
            return Ok(trainer);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPatch("update")]
    public IActionResult UpdateTrainerName([FromBody] UpdateDTO trainer)
    {
        try
        {
            var updated = _trainerService.UpdateTrainer(trainer);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return Conflict(ex.Message);
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