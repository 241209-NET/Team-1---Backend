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
    /*[Fact]
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

        Pkmn newPkmn = new Pkmn{Species = "Dialga", Name = "Ea"};

        mockPkmnRepo.Setup(repo => repo.CreateNewPkmn(It.IsAny<Pkmn>()))
            .Callback((Pkmn p) => pkmnList.Add(p))
            .Returns(newPkmn);

        // Act
        var myPkmn = pkmnService.CreateNewPkmn(mapper.Map<PkmnInDTO>(newPkmn));

        // Assert
        Assert.Contains(newPkmn, pkmnList);
        mockPkmnRepo.Verify(p => p.CreateNewPkmn(It.IsAny<Pkmn>()), Times.Once());
    }*/

    /*[Fact]
    public void GetAllPkmnTest()
    {
        // Arrange
        Mock<IPokemonRepository> mockPkmnRepo = new();
        PokemonService pkmnService = new(mockPkmnRepo.Object);

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

        // Assert
        Assert.Equal(pkmnList, result);
    }*/

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

    
    /*[Fact]
    public void GetAllPkmnBySpeciesTest()
    {
        // Arrange
        Mock<IPokemonRepository> mockPkmnRepo = new();
        PokemonService pkmnService = new(mockPkmnRepo.Object);

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

        // Assert
        Assert.Equal(speciesList, result);
    }*/

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
}