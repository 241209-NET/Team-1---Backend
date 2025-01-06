using Microsoft.EntityFrameworkCore;
using PokemonTracker.API.Model;

namespace PokemonTracker.API.Data;

public partial class PokemonContext : DbContext
{
    public PokemonContext(){}
    public PokemonContext(DbContextOptions<PokemonContext> options) : base(options){}

    public virtual DbSet<Pkmn> Pkmns { get; set; }
    public virtual DbSet<Trainer> Trainers { get; set; }
}