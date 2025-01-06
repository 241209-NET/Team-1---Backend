using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace PokemonTracker.API.Model;

[Index(nameof(Name), IsUnique = true)]
public class Pkmn
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Species { get; set; } = "";
    public string Name { get; set; } = "";

    public string Type { get; set;} = "";
    public int TrainerID { get; set; }
}