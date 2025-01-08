using Moq;
using AutoMapper;
using PokemonTracker.API.Model;
using PokemonTracker.API.DTO;
using PokemonTracker.API.Repository;
using PokemonTracker.API.Service;
using Xunit.Sdk;
namespace PokemonTracker.Test;

public class TrainerServiceTests
{
    // [Fact]
    // public void CreateNewTrainerTest()
    // {
    //     // Arrange
    //     Mock<ITrainerRepository> mockTrainerRepo = new();
    //     TrainerService trainerService = new(mockTrainerRepo.Object);

    //     List<Trainer> trainerList = [
    //         new Trainer{Name = "Ash"},
    //         new Trainer{Name = "Brock"},
    //         new Trainer{Name = "Misty"}
    //     ];

    //     Trainer newTrainer = new Trainer{Name = "Tracy"};

    //     mockTrainerRepo.Setup(repo => repo.CreateNewTrainer(It.IsAny<Trainer>()))
    //         .Callback((Trainer t) => trainerList.Add(t))
    //         .Returns(newTrainer);

    //     // Act
    //     var myTrainer = trainerService.CreateNewTrainer(newTrainer);

    //     // Assert
    //     Assert.Contains(newTrainer, trainerList);
    //     mockTrainerRepo.Verify(t => t.CreateNewTrainer(It.IsAny<Trainer>()), Times.Once());
    // }

//     [Fact]
//     public void GetAllTrainersTest()
//     {
//         // Arrange
//         Mock<ITrainerRepository> mockRepo = new();
//         TrainerService trainerService = new(mockRepo.Object);

//         List<Trainer> trainerList = [
//             new Trainer{Name = "Ash"},
//             new Trainer{Name = "Brock"},
//             new Trainer{Name = "Misty"}
//         ];

//         mockRepo.Setup(repo => repo.GetAllTrainers()).Returns(trainerList);

//         // Act
//         var result = trainerService.GetAllTrainers().ToList();

//         // Assert
//         Assert.Equal(trainerList, result);
//     }

//     [Fact]
//     public void GetTrainerByName()
//     {
//         // Arrange
//         Mock<ITrainerRepository> mockRepo = new();
//         TrainerService trainerService = new(mockRepo.Object);

//         Trainer newTrainer = new Trainer{Name = "Serena"};

//         mockRepo.Setup(repo => repo.GetTrainerByName("Serena")).Returns(newTrainer);

//         // Act
//         var result = trainerService.GetTrainerByName("Serena");

//         // Assert
//         Assert.Equal(newTrainer, result);
//     }

    [Fact]
    public void DeleteTrainerByNameTest()
    {
        // Arrange
        Mock<ITrainerRepository> mockTrainerRepo = new();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        TrainerService trainerService = new(mockTrainerRepo.Object, mockMapper.Object);

        // Configure Automapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        IMapper mapper = config.CreateMapper();

        // mock the mapper to see if mapped successfully to all fields of an OutDTO,
        // then, wherever in this test we pass in a Trainer object, will substitute it with 
        // a TrainerOutDTO instead.
        mockMapper.Setup(m => m.Map<TrainerOutDTO>(It.IsAny<Trainer>())).Returns((Trainer t) => new TrainerOutDTO
        {
            Id = t.Id,
            Name = t.Name,
            Team = t.Team.Select(p => new PkmnOutDTO()).ToList()  
        });

        // mock list of trainers
        List<Trainer> trainerList = [
            new Trainer{ Name = "Ash" },
            new Trainer{ Name = "Brock" },
            new Trainer{ Name = "Misty" }
        ];

        // mock trainer to be deleted
        Trainer deletedTrainer = trainerList.First(t => t.Name == "Misty");

        // mock get trainer by name since we use it in service for delete method.
        mockTrainerRepo.Setup(repo => repo.GetTrainerByName(It.IsAny<string>()))
            .Returns((string name) => trainerList.FirstOrDefault(t => t.Name == name));

        // mock delete trainer by name for actual deletion
        mockTrainerRepo.Setup(repo => repo.DeleteTrainerByName(It.Is<Trainer>(t => t.Name == "Misty")))
            .Returns((Trainer trainer) => 
            {
                trainerList.Remove(trainer);        // removing it from list
                return trainer;
            });

        // Act
        var deletedTrainerDto = trainerService.DeleteTrainerByName(deletedTrainer.Name);

        // Assert
        Assert.NotNull(deletedTrainerDto);          
        Assert.Equal(deletedTrainer.Name, deletedTrainerDto.Name);  // our Misty is same as our created dto name
        Assert.DoesNotContain(deletedTrainer, trainerList);     // is actually removed
    }
}