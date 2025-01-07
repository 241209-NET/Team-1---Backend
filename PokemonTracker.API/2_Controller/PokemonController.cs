namespace PokemonTracker.API.Controller;

using Microsoft.AspNetCore.Mvc;
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
    public IActionResult CreateNewPkmn(Pkmn newPkmn)
    {
        var pkmn = _pokemonService.CreateNewPkmn(newPkmn);

        if (pkmn is null)
        {
            return NotFound();
        }

        return Ok(pkmn);
    }

    [HttpDelete("delete/{name}")]
    public IActionResult DeletePkmnByName(string name)
    {
        var deletePkmn = _pokemonService.DeletePkmnByName(name);

        if (deletePkmn is null)
        {
            return NotFound();
        }

        return Ok(deletePkmn);
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
        var findPkmn = _pokemonService.GetPkmnByName(name);

        if (findPkmn is null)
        {
            return NotFound();
        }

        return Ok(findPkmn);
    }

    [HttpGet("species/{species}")]
    public IActionResult GetPkmnBySpecies(string species)
    {
        var findPkmn = _pokemonService.GetAllPkmnBySpecies(species);

        if (findPkmn is null)
        {
            return NotFound();
        }

        return Ok(findPkmn);
    }

    [HttpGet("type/{type}")]
    public IActionResult GetPkmnByType(string type)
    {
        var findPkmn = _pokemonService.GetAllPkmnByType(type);

        if (findPkmn is null)
        {
            return NotFound();
        }

        return Ok(findPkmn);
    }

}