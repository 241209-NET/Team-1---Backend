using AutoMapper;
using Azure.Core;
using Moq;
using PokemonTracker.API.DTO;
using PokemonTracker.API.Model;
using PokemonTracker.API.Repository;
using PokemonTracker.API.Service;
namespace PokemonTracker.Test;

public class PkmnServiceTests
{
    [Fact]
    public void CreateNewPkmnTest()
    {
        // Arrange
        Mock<IPokemonRepository> mockPkmnRepo = new();
        Mock<ITrainerRepository> mockTrainerRepo = new();
        //Configure Automapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        IMapper mapper = config.CreateMapper();

        TrainerService ts = new(mockTrainerRepo.Object, mapper);
        PokemonService pkmnService = new(mockPkmnRepo.Object, mapper, ts);

        List<Pkmn> pkmnList = [
            new Pkmn{Species = "Bulbasaur", Name = "Ivy"},
            new Pkmn{Species = "Charmander", Name = "Charles"},
            new Pkmn{Species = "Gengar", Name = "Chaolan"},
            new Pkmn{Species = "Machamp", Name = "Fox"}
        ];

        Trainer newTrainer = new Trainer{Id = 0, Name = "Jeff"};

        mockTrainerRepo.Setup(repo => repo.CreateNewTrainer(It.IsAny<Trainer>())).Returns(newTrainer);
        mockTrainerRepo.Setup(repo => repo.GetTeam(It.IsAny<string>())).Returns([newTrainer]);
        mockTrainerRepo.Setup(repo => repo.GetTrainerById(It.IsAny<int>())).Returns(newTrainer);

        TrainerOutDTO t = ts.CreateNewTrainer(mapper.Map<TrainerInDTO>(newTrainer));

        Pkmn newPkmn = new Pkmn{Species = "Dialga", Name = "Ea", TrainerID = t.Id};

        mockPkmnRepo.Setup(repo => repo.CreateNewPkmn(It.IsAny<Pkmn>()))
            .Callback((Pkmn p) => pkmnList.Add(p))
            .Returns(newPkmn);

        // Act
        var myPkmn = pkmnService.CreateNewPkmn(mapper.Map<PkmnInDTO>(newPkmn));

        Pkmn convert = mapper.Map<Pkmn>(myPkmn);
        var found = pkmnList.Find(p => p.Name.Equals(convert.Name));
        // Assert
        Assert.Equal(myPkmn.Name, found.Name);
        mockPkmnRepo.Verify(p => p.CreateNewPkmn(It.IsAny<Pkmn>()), Times.Once());
    }

    [Fact]
    public void CreateNewPkmnTeamFullTest()
    {
        // Arrange
        Mock<IPokemonRepository> mockPkmnRepo = new();
        Mock<ITrainerRepository> mockTrainerRepo = new();
        //Configure Automapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        IMapper mapper = config.CreateMapper();

        TrainerService ts = new(mockTrainerRepo.Object, mapper);
        PokemonService pkmnService = new(mockPkmnRepo.Object, mapper, ts);

        Trainer newTrainer = new Trainer{Id = 0, Name = "Kyle"};

        mockTrainerRepo.Setup(repo => repo.CreateNewTrainer(It.IsAny<Trainer>())).Returns(newTrainer);

        List<Pkmn> pkmnList = [
            new Pkmn{Species = "Bulbasaur", Name = "Ivy"},
            new Pkmn{Species = "Charmander", Name = "Charles"},
            new Pkmn{Species = "Gengar", Name = "Chaolan"},
            new Pkmn{Species = "Machamp", Name = "Fox"},
            new Pkmn{Species = "Machamp", Name = "Harpy"},
            new Pkmn{Species = "Machamp", Name = "Frank"},
        ];

         var t = ts.CreateNewTrainer(mapper.Map<TrainerInDTO>(newTrainer));
        Pkmn newPkmn = new Pkmn{Species = "Bulbasaur", Name = "Rich", TrainerID = t.Id};

        t.Team = mapper.Map<List<PkmnOutDTO>>(pkmnList);

        mockTrainerRepo.Setup(repo => repo.GetTeam(It.IsAny<string>())).Returns([newTrainer]);
        mockTrainerRepo.Setup(repo => repo.GetTrainerById(It.IsAny<int>())).Returns(newTrainer);
        mockPkmnRepo.Setup(repo => repo.CreateNewPkmn(It.IsAny<Pkmn>())).Returns(newPkmn);

        // Act
        Action pkmnSeven = () => pkmnService.CreateNewPkmn(mapper.Map<PkmnInDTO>(newPkmn));

        Assert.Empty("SIZE " + t.Team.Count);
        
        Exception exception = Assert.Throws<Exception>(pkmnSeven);

        // Assert
        Assert.Equal("This trainer's team is already full! Please remove a pokemon first.", exception.Message);
    }

    [Fact]
    public void UpdatePkmnTest()
    {

    }

    [Fact]
    public void GetAllPkmnTest()
    {
        // Arrange
        Mock<IPokemonRepository> mockPkmnRepo = new();
        Mock<ITrainerRepository> mockTrainerRepo = new();
        //Configure Automapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        IMapper mapper = config.CreateMapper();

        TrainerService ts = new(mockTrainerRepo.Object, mapper);
        PokemonService pkmnService = new(mockPkmnRepo.Object, mapper, ts);

        List<Pkmn> pkmnList = [
            new Pkmn{Species = "Bulbasaur", Name = "Ivy"},
            new Pkmn{Species = "Bulbasaur", Name = "Venus"},
            new Pkmn{Species = "Charmander", Name = "Charles"},
            new Pkmn{Species = "Gengar", Name = "Chaolan"},
            new Pkmn{Species = "Machamp", Name = "Fox"}
        ];

        mockPkmnRepo.Setup(repo => repo.GetAllPkmn()).Returns(pkmnList);

        // Act
        var result = pkmnService.GetAllPkmn().ToList();

        var convertPkmnList = mapper.Map<List<PkmnOutDTO>>(pkmnList);

        // Assert
        Assert.Equivalent(convertPkmnList, result);
    }

    [Fact]
    public void DeletePkmnTest()
    {
        // Arrange
        Mock<IPokemonRepository> mockPkmnRepo = new();
        Mock<ITrainerRepository> mockTrainerRepo = new();

        // Configure Automapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        IMapper mapper = config.CreateMapper();

        TrainerService ts = new(mockTrainerRepo.Object, mapper);
        PokemonService pkmnService = new(mockPkmnRepo.Object, mapper, ts);

        List<Pkmn> pkmnList = new List<Pkmn>
        {
            new Pkmn { Id = 0, Species = "Bulbasaur", Name = "Ivy" },
            new Pkmn { Id = 1, Species = "Bulbasaur", Name = "Venus" },
            new Pkmn { Id = 2, Species = "Charmander", Name = "Charles" },
            new Pkmn { Id = 3, Species = "Gengar", Name = "Chaolan" },
            new Pkmn { Id = 4, Species = "Machamp", Name = "Fox" }
        };

        Pkmn newPkmn = new Pkmn { Id = 2, Species = "Charmander", Name = "Charles" };

        //mockPkmnRepo.Setup(repo => repo.GetPkmnByName(It.IsAny<string>()))
               // .Returns((string name) => pkmnList.Find(p => p.Name == name));

        mockPkmnRepo.Setup(repo => repo.DeletePkmn(It.IsAny<Pkmn>()))
                .Callback<Pkmn>(pkmn => pkmnList.Remove(pkmn)).Returns(newPkmn);

        // Act
        var deletedPkmn = mapper.Map<Pkmn>(pkmnService.DeletePkmn(newPkmn.Id));

        // Assert
        Assert.NotNull(deletedPkmn);
        Assert.Equal(newPkmn.Name, deletedPkmn.Name);
        Assert.DoesNotContain(deletedPkmn, pkmnList); // Ensure the Pokémon is removed from the list
        //mockPkmnRepo.Verify(x => x.GetPkmnByName(It.IsAny<string>()), Times.Once());
        mockPkmnRepo.Verify(x => x.DeletePkmn(It.IsAny<Pkmn>()), Times.Once());
    }
}