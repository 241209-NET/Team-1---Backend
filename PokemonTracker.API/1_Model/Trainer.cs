using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace PokemonTracker.API.Model;

[Index(nameof(Name), IsUnique = true)]
public class Trainer
{
    public int Id { get; set;}
    public string Name { get; set;} = "";

    public List<Pkmn> Team { get; set; } = [];
}
