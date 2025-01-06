using Moq;
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
        PokemonService pkmnService = new(mockPkmnRepo.Object);

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
        var myPkmn = pkmnService.CreateNewPkmn(newPkmn);

        // Assert
        Assert.Contains(newPkmn, pkmnList);
        mockPkmnRepo.Verify(p => p.CreateNewPkmn(It.IsAny<Pkmn>()), Times.Once());
    }

    [Fact]
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
    }

    [Fact]
    public void GetPkmnByName()
    {
        // Arrange
        Mock<IPokemonRepository> mockRepo = new();
        PokemonService pkmnService = new(mockRepo.Object);

        Pkmn newPkmn = new Pkmn{Species = "Dialga", Name = "Ea"};

        mockRepo.Setup(repo => repo.GetPkmnByName("Dialga")).Returns(newPkmn);

        // Act
        var result = pkmnService.GetPkmnByName("Dialga");

        // Assert
        Assert.Equal(newPkmn, result);
    }

    
    [Fact]
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
    }

    [Fact]
    public void DeletePkmnByNameTest()
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

        Pkmn newPkmn = new Pkmn{Species = "Charmander", Name = "Charles"};

        mockPkmnRepo.Setup(repo => repo.DeletePkmnByName(It.IsAny<Pkmn>()))
            .Callback((Pkmn p) => pkmnList.Remove(p))
            .Returns(newPkmn);

        // Act
        var myPkmn = pkmnService.DeletePkmnByName(newPkmn.Name);

        // Assert
        Assert.DoesNotContain(myPkmn, pkmnList);
  }
}