namespace PokemonTracker.API.Controller;

using Microsoft.AspNetCore.Mvc;
using PokemonTracker.API.DTO;
using PokemonTracker.API.Model;
using PokemonTracker.API.Service;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : ControllerBase
{
    private readonly IPokemonService _pokemonService;

    public PokemonController(IPokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    [HttpPost]
    public IActionResult CreateNewPkmn(PkmnInDTO newPkmn)
    {
        try
        {
            var pkmn = _pokemonService.CreateNewPkmn(newPkmn);
            return Ok(pkmn);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }       
    }

    [HttpDelete("delete/{name}")]
    public IActionResult DeletePkmnByName(string name)
    {
        try
        {
            var deletePkmn = _pokemonService.DeletePkmnByName(name);
            return Ok(deletePkmn);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetAllPkmn()
    {
        var pkmnList = _pokemonService.GetAllPkmn();
        return Ok(pkmnList);
    }

    [HttpGet("name/{name}")]
    public IActionResult GetPkmnByName(string name)
    {
        try
        {
            var findPkmn = _pokemonService.GetPkmnByName(name);
            return Ok(findPkmn);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("species/{species}")]
    public IActionResult GetPkmnBySpecies(string species)
    {
        try
        {
            var findPkmn = _pokemonService.GetAllPkmnBySpecies(species);

            return Ok(findPkmn);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("type/{type}")]
    public IActionResult GetPkmnByType(string type)
    {
        try
        {
            var findPkmn = _pokemonService.GetAllPkmnByType(type);
            return Ok(findPkmn);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}