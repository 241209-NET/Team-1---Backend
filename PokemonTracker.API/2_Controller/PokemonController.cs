namespace PokemonTracker.API.Controller;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
    public IActionResult CreateNewPkmn([FromBody] PkmnInDTO newPkmn)
    {
        try
        {
            var pkmn = _pokemonService.CreateNewPkmn(newPkmn);
            return Ok(pkmn);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("entity changes"))
            {
                return Conflict("A Pokemon with this nickname already exists!");
            }
            else
            {
                return BadRequest(ex.Message);
            }   
        }       
    }

    [HttpPatch("update")]
    public IActionResult UpdatePkmnName([FromBody] UpdateDTO pkmn)
    {
        try
        {
            var updated = _pokemonService.UpdatePkmn(pkmn);
            return Ok(updated);
        }
        catch (Exception)
        {
            return Conflict("A Pokemon with this nickname already exists!");
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult DeletePkmn(int id)
    {
        try
        {
            var deletePkmn = _pokemonService.DeletePkmn(id);
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
}