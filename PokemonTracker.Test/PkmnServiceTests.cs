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
    public void CreateNewPkmnAlreadyExistsTest()
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

        Trainer newTrainer = new Trainer{Id = 0, Name = "Harry"};

        mockTrainerRepo.Setup(repo => repo.CreateNewTrainer(It.IsAny<Trainer>())).Returns(newTrainer);
        mockTrainerRepo.Setup(repo => repo.GetTeam(It.IsAny<string>())).Returns([newTrainer]);
        mockTrainerRepo.Setup(repo => repo.GetTrainerById(It.IsAny<int>())).Returns(newTrainer);

        var t = ts.CreateNewTrainer(mapper.Map<TrainerInDTO>(newTrainer));

        Pkmn newPkmn = new Pkmn{Species = "Bulbasaur", Name = "Ivy", TrainerID = t.Id};

        mockPkmnRepo.Setup(repo => repo.CreateNewPkmn(It.IsAny<Pkmn>())).Returns(newPkmn);
        //mockPkmnRepo.Setup(repo => repo.GetPkmnByName(It.IsAny<string>())).Returns(newPkmn);

        // Act
        var mapPkmn = mapper.Map<PkmnInDTO>(newPkmn);
        Action exists = () => pkmnService.CreateNewPkmn(mapPkmn);

        Exception exception = Assert.Throws<Exception>(exists);

        // Assert
        Assert.Equal("This Pokemon already exists!", exception.Message);
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
        mockTrainerRepo.Setup(repo => repo.GetTeam(It.IsAny<string>())).Returns([newTrainer]);
        mockTrainerRepo.Setup(repo => repo.GetTrainerById(It.IsAny<int>())).Returns(newTrainer);

        var t = ts.CreateNewTrainer(mapper.Map<TrainerInDTO>(newTrainer));

        List<Pkmn> pkmnList = [
            new Pkmn{Species = "Bulbasaur", Name = "Ivy"},
            new Pkmn{Species = "Charmander", Name = "Charles"},
            new Pkmn{Species = "Gengar", Name = "Chaolan"},
            new Pkmn{Species = "Machamp", Name = "Fox"},
            new Pkmn{Species = "Machamp", Name = "Harpy"},
            new Pkmn{Species = "Machamp", Name = "Frank"}
        ];

        t.Team = mapper.Map<List<PkmnOutDTO>>(pkmnList);

        // Act
        Action pkmnSeven = () => pkmnService.CreateNewPkmn(mapper.Map<PkmnInDTO>(new Pkmn{Species = "Bulbasaur", Name = "Rich", TrainerID = t.Id}));

        Exception exception = Assert.Throws<Exception>(pkmnSeven);

        // Assert
        Assert.Equal("This trainer's team is already full! Please remove a pokemon first.", exception.Message);
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
<<<<<<< HEAD
=======

    [Fact]
    public void GetAllPkmnBySpeciesTest()
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

        List<Pkmn> speciesList = [
            new Pkmn{Species = "Bulbasaur", Name = "Ivy"},
            new Pkmn{Species = "Bulbasaur", Name = "Venus"}
        ];

        mockPkmnRepo.Setup(repo => repo.GetAllPkmnBySpecies("bulbasaur")).Returns(speciesList);

        // Act
        var result = pkmnService.GetAllPkmnBySpecies("bulbasaur").ToList();

        var convertList = mapper.Map<List<PkmnOutDTO>>(speciesList);

        // Assert
        Assert.Equivalent(convertList, result);
    }

    [Fact]
    public void GetPkmnByName()
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

        Pkmn newPkmn = new Pkmn{Species = "Dialga", Name = "Ea"};

        mockPkmnRepo.Setup(repo => repo.GetPkmnByName(It.IsAny<string>())).Returns(newPkmn);

        // Act
        var result = mapper.Map<Pkmn>(pkmnService.GetPkmnByName("Dialga"));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newPkmn.Species, result.Species); // Compare the Species property
        Assert.Equal(newPkmn.Name, result.Name);       // Compare the Name property
        mockPkmnRepo.Verify(x => x.GetPkmnByName(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public void DeletePkmnByNameTest()
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
            new Pkmn { Species = "Bulbasaur", Name = "Ivy" },
            new Pkmn { Species = "Bulbasaur", Name = "Venus" },
            new Pkmn { Species = "Charmander", Name = "Charles" },
            new Pkmn { Species = "Gengar", Name = "Chaolan" },
            new Pkmn { Species = "Machamp", Name = "Fox" }
        };

        Pkmn newPkmn = new Pkmn { Species = "Charmander", Name = "Charles" };

        mockPkmnRepo.Setup(repo => repo.GetPkmnByName(It.IsAny<string>()))
                .Returns((string name) => pkmnList.Find(p => p.Name == name));

        mockPkmnRepo.Setup(repo => repo.DeletePkmnByName(It.IsAny<Pkmn>()))
                .Callback<Pkmn>(pkmn => pkmnList.Remove(pkmn));

        // Act
        var deletedPkmn = mapper.Map<Pkmn>(pkmnService.DeletePkmnByName(newPkmn.Name));

        // Assert
        Assert.NotNull(deletedPkmn);
        Assert.Equal(newPkmn.Name, deletedPkmn.Name);
        Assert.DoesNotContain(deletedPkmn, pkmnList); // Ensure the Pokémon is removed from the list
        mockPkmnRepo.Verify(x => x.GetPkmnByName(It.IsAny<string>()), Times.Once());
        mockPkmnRepo.Verify(x => x.DeletePkmnByName(It.IsAny<Pkmn>()), Times.Once());
    }

    [Fact]
    public void DeletePkmnByNameTestNullCheck()
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
            new Pkmn { Species = "Bulbasaur", Name = "Ivy" },
            new Pkmn { Species = "Bulbasaur", Name = "Venus" },
            new Pkmn { Species = "Charmander", Name = "Charles" },
            new Pkmn { Species = "Gengar", Name = "Chaolan" },
            new Pkmn { Species = "Machamp", Name = "Fox" }
        };

        Pkmn newPkmn = new Pkmn { Species = "Charmander", Name = "Frank" };

        mockPkmnRepo.Setup(repo => repo.GetPkmnByName(It.IsAny<string>()))
                .Returns((string name) => pkmnList.Find(p => p.Name == name));

        mockPkmnRepo.Setup(repo => repo.DeletePkmnByName(It.IsAny<Pkmn>()))
                .Callback<Pkmn>(pkmn => pkmnList.Remove(pkmn));

        // Act
        var deletedPkmn = mapper.Map<Pkmn>(pkmnService.DeletePkmnByName(newPkmn.Name));

        // Assert
        Assert.Null(deletedPkmn);
        mockPkmnRepo.Verify(x => x.GetPkmnByName(It.IsAny<string>()), Times.Once());
    }
>>>>>>> origin/main
}