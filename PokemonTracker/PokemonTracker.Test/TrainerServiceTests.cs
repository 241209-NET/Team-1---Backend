using Moq;
using PokemonTracker.API.Model;
using PokemonTracker.API.Repository;
using PokemonTracker.API.Service;
using Xunit.Sdk;
namespace PokemonTracker.Test;

public class TrainerServiceTests
{
    [Fact]
    public void CreateNewTrainerTest()
    {
        // Arrange
        Mock<ITrainerRepository> mockTrainerRepo = new();
        TrainerService trainerService = new(mockTrainerRepo.Object);

        List<Trainer> trainerList = [
            new Trainer{Name = "Ash"},
            new Trainer{Name = "Brock"},
            new Trainer{Name = "Misty"}
        ];

        Trainer newTrainer = new Trainer{Name = "Tracy"};

        mockTrainerRepo.Setup(repo => repo.CreateNewTrainer(It.IsAny<Trainer>()))
            .Callback((Trainer t) => trainerList.Add(t))
            .Returns(newTrainer);

        // Act
        var myTrainer = trainerService.CreateNewTrainer(newTrainer);

        // Assert
        Assert.Contains(newTrainer, trainerList);
        mockTrainerRepo.Verify(t => t.CreateNewTrainer(It.IsAny<Trainer>()), Times.Once());
    }

    [Fact]
    public void GetAllTrainersTest()
    {
        // Arrange
        Mock<ITrainerRepository> mockRepo = new();
        TrainerService trainerService = new(mockRepo.Object);

        List<Trainer> trainerList = [
            new Trainer{Name = "Ash"},
            new Trainer{Name = "Brock"},
            new Trainer{Name = "Misty"}
        ];

        mockRepo.Setup(repo => repo.GetAllTrainers()).Returns(trainerList);

        // Act
        var result = trainerService.GetAllTrainers().ToList();

        // Assert
        Assert.Equal(trainerList, result);
    }

    [Fact]
    public void GetTrainerByName()
    {
        // Arrange
        Mock<ITrainerRepository> mockRepo = new();
        TrainerService trainerService = new(mockRepo.Object);

        Trainer newTrainer = new Trainer{Name = "Serena"};

        mockRepo.Setup(repo => repo.GetTrainerByName("Serena")).Returns(newTrainer);

        // Act
        var result = trainerService.GetTrainerByName("Serena");

        // Assert
        Assert.Equal(newTrainer, result);
    }

    [Fact]
    public void DeleteTrainerByNameTest()
    {
        // Arrange
        Mock<ITrainerRepository> mockRepo = new();
        TrainerService trainerService = new(mockRepo.Object);

        List<Trainer> trainerList = [
            new Trainer{Name = "Ash"},
            new Trainer{Name = "Brock"},
            new Trainer{Name = "Misty"}
        ];

        Trainer newTrainer = new Trainer{Name = "Misty"};

        mockRepo.Setup(repo => repo.DeleteTrainerByName(It.IsAny<Trainer>()))
            .Callback((Trainer t) => trainerList.Remove(t))
            .Returns(newTrainer);

        // Act
        var myTrainer = trainerService.DeleteTrainerByName(newTrainer.Name);

        // Assert
        Assert.DoesNotContain(myTrainer, trainerList);
  }
}