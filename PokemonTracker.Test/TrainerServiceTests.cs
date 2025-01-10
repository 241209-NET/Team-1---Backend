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









































































































































































































    [Fact]
    public void GetTrainerByName_Exists_Test()
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

        // mock trainer to be got:
        Trainer chosenTrainer = new Trainer { Name = "Serena" };

        // mock get trainer by name:
        mockTrainerRepo.Setup(repo => repo.GetTrainerByName("Serena"))
            .Returns(chosenTrainer);

        // Act
        var res = trainerService.GetTrainerByName("Serena");

        // Assert
        Assert.NotNull(res);
        // check each field of dto to make sure success of dto mapping.
        // cannot do Assert.Equal(chosenTrainer, res) cuz 1 is Trainer obj and res is dto obj.
        Assert.Equal(chosenTrainer.Name, res.Name);     
        Assert.Equal(chosenTrainer.Id, res.Id);         
        Assert.Equal(chosenTrainer.Team.Count, res.Team.Count);
    }





    [Fact]                  ///////// need to revise for 
    public void GetTrainerByName_DoesntExist_Test()
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

        // // mock the mapper to see if mapped successfully to all fields of an OutDTO,
        // // then, wherever in this test we pass in a Trainer object, will substitute it with 
        // // a TrainerOutDTO instead.
        // mockMapper.Setup(m => m.Map<TrainerOutDTO>(It.IsAny<Trainer>())).Returns((Trainer t) => new TrainerOutDTO
        // {
        //     Id = t.Id,
        //     Name = t.Name,
        //     Team = t.Team.Select(p => new PkmnOutDTO()).ToList()
        // });

        // // mock trainer to be got:
        // Trainer chosenTrainer = new Trainer { Name = "Serena" };

        // // mock get trainer by name:
        // mockTrainerRepo.Setup(repo => repo.GetTrainerByName("Serena"))
        //     .Returns(chosenTrainer);

        // // Act
        // var res = trainerService.GetTrainerByName("Serena");

        // // Assert
        // Assert.NotNull(res);
        // // check each field of dto to make sure success of dto mapping.
        // // cannot do Assert.Equal(chosenTrainer, res) cuz 1 is Trainer obj and res is dto obj.
        // Assert.Equal(chosenTrainer.Name, res.Name);
        // Assert.Equal(chosenTrainer.Id, res.Id);
        // Assert.Equal(chosenTrainer.Team.Count, res.Team.Count);
    }







    [Fact]
    public void DeleteTrainerByName_Exists_Test()
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
        mockTrainerRepo.Setup(repo => repo.GetTrainerByName("Misty"))
            .Returns((string name) => trainerList.FirstOrDefault(t => t.Name == "Misty"));

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


    [Fact]
    public void DeleteTrainerByName_DoesntExist_Test()
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

        // mock a non-existent trainer name
        string ghostTrainerName = "Grunch";

        // mock get trainer by name since we use it in service for delete method.
        // should use var name instead of literal string Grunch here.
        mockTrainerRepo.Setup(repo => repo.GetTrainerByName(It.Is<string>(name => name == ghostTrainerName)))
            .Returns((string name) => trainerList.FirstOrDefault(t => t.Name == name));

        // don't mock delete trainer by name for deletion since we will halt at failed get by name.

        // Act
        var deletedTrainerDto = trainerService.DeleteTrainerByName(ghostTrainerName);

        // Assert
        Assert.Null(deletedTrainerDto);
    }
}