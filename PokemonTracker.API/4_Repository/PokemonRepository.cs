using PokemonTracker.API.Data;
using PokemonTracker.API.Model;

namespace PokemonTracker.API.Repository;

public class PokemonRepository : IPokemonRepository
{
    private readonly PokemonContext _pokemonContext;
    
    public PokemonRepository(PokemonContext pokemonContext)
    {
        _pokemonContext = pokemonContext;
    }

    public Pkmn CreateNewPkmn(Pkmn newPkmn)
    {
        _pokemonContext.Pkmns.Add(newPkmn);
        _pokemonContext.SaveChanges();
        return newPkmn;
    }

    public Pkmn UpdatePkmn(Pkmn update)
    {
        _pokemonContext.Pkmns.Update(update);
        _pokemonContext.SaveChanges();
        return update;
    }

    public Pkmn? DeletePkmn(Pkmn pkmn)
    {
        _pokemonContext.Pkmns.Remove(pkmn);
        _pokemonContext.SaveChanges();
        
        return pkmn;
    }
    
    public Pkmn GetPkmnById(int id)
    {
        Pkmn pkmn = _pokemonContext.Pkmns.Find(id)!;
        return pkmn;
    }

    public IEnumerable<Pkmn> GetAllPkmn()
    {
        return _pokemonContext.Pkmns.ToList();
    }
}