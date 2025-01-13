using Microsoft.EntityFrameworkCore;

namespace PokemonTracker.API.DTO;

[Index(nameof(TrainerID), nameof(Name), IsUnique = true)]
public class PkmnInDTO
{
    public string Species { get; set; } = "";
    public string Name { get; set; } = "";
    public int TrainerID { get; set; }
}

public class PkmnOutDTO
{
    public int Id { get; set; }
    public string Species { get; set; } = "";
    public string Name { get; set; } = "";
}

[Index(nameof(Username), IsUnique = true)]
public class TrainerInDTO
{
    public string Name { get; set;} = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

[Index(nameof(Username), IsUnique = true)]
public class LoginDTO
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class UpdateDTO
{
    public int Id { get; set;}
     public string Name { get; set;} = "";
}

public class TrainerOutDTO
{
    public int Id { get; set;}
    public string Name { get; set;} = "";
    public List<PkmnOutDTO> Team { get; set; } = [];
}