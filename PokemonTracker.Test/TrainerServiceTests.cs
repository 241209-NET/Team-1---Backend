using AutoMapper;
using Moq;
using PokemonTracker.API.DTO;
using PokemonTracker.API.Model;
using PokemonTracker.API.Repository;
using PokemonTracker.API.Service;
namespace PokemonTracker.Test;

public class TrainerServiceTests
{
    [Fact]
    public void CreateNewTrainerTest()
    {
        // Arrange
        Mock<ITrainerRepository> mockTrainerRepo = new();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        IMapper mapper = config.CreateMapper();
        TrainerService trainerService = new(mockTrainerRepo.Object, mapper);

        List<Trainer> trainerList = [
            new Trainer{Name = "Ash"},
            new Trainer{Name = "Brock"},
            new Trainer{Name = "Misty"}
        ];

        Trainer newTrainer = new Trainer { Name = "Tracy" };

        mockTrainerRepo.Setup(repo => repo.CreateNewTrainer(It.IsAny<Trainer>()))
            .Callback(() => trainerList.Add(newTrainer))
            .Returns(newTrainer);

        // Act
        var myTrainer = trainerService.CreateNewTrainer(mapper.Map<TrainerInDTO>(newTrainer));

        // Assert
        Assert.Contains(newTrainer, trainerList);
        Assert.Equal(myTrainer.Name, newTrainer.Name);
        mockTrainerRepo.Verify(t => t.CreateNewTrainer(It.IsAny<Trainer>()), Times.Once());
    }


    [Fact]
    public void GetAllTrainersTest()
    {
        // Arrange
        Mock<ITrainerRepository> mockRepo = new();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        IMapper mapper = config.CreateMapper();
        TrainerService trainerService = new(mockRepo.Object, mapper);

        List<Trainer> trainerList = [
            new Trainer{Name = "Ash"},
            new Trainer{Name = "Brock"},
            new Trainer{Name = "Misty"}
        ];

        mockRepo.Setup(repo => repo.GetAllTrainers()).Returns(trainerList);

        // Act
        var result = mapper.Map<List<Trainer>>(trainerService.GetAllTrainers().ToList());

        // Assert
        for (int i = 0; i < trainerList.Count; i++)
        {
            Assert.Equal(result[i].Name, trainerList[i].Name);
        }
    }


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


    [Fact]
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

        // mock get trainer by name 
        // should use var name instead of literal string Grunch here.
        mockTrainerRepo.Setup(repo => repo.GetTrainerByName(It.Is<string>(name => name == ghostTrainerName)))
            .Returns((string name) => trainerList.FirstOrDefault(t => t.Name == name));

        // Act
        var exception = Assert.Throws<Exception>(() => trainerService.DeleteTrainerByName(ghostTrainerName));

        // Assert
        Assert.Equal("This trainer doesn't exist!", exception.Message);
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
        var exception = Assert.Throws<Exception>(() => trainerService.DeleteTrainerByName(ghostTrainerName));

        // Assert
        Assert.Equal("This trainer doesn't exist!", exception.Message);
    }


    [Fact]
    public void GetTeam_Test()
    {
        // Arrange
        var mockTrainerRepo = new Mock<ITrainerRepository>();
        var mockMapper = new Mock<IMapper>();
        var trainerService = new TrainerService(mockTrainerRepo.Object, mockMapper.Object);

        // Sample Trainer with associated Team (list of Pokémon)
        var trainer = new Trainer
        {
            Id = 1,
            Name = "Ash",
            Team = new List<Pkmn>
            {
                new Pkmn { Name = "Pikachu", Species = "Electric", DexNumber = 7 },
                new Pkmn { Name = "Charizard", Species = "Fire", DexNumber = 17 }
            }
        };

        // Mock the repository to return the trainer, specifically "Ash"
        mockTrainerRepo.Setup(repo => repo.GetTeam("Ash")).Returns(new List<Trainer> { trainer });

        // Mock the mapper correctly for IEnumerable<TrainerOutDTO> instead of TrainerOutDTO
        mockMapper.Setup(m => m.Map<IEnumerable<TrainerOutDTO>>(It.IsAny<IEnumerable<Trainer>>()))
            .Returns((IEnumerable<Trainer> trainers) => trainers.Select(t => new TrainerOutDTO
            {
                Id = t.Id,
                Name = t.Name,
                Team = t.Team.Select(p => new PkmnOutDTO
                {
                    Name = p.Name,       // Map Pokémon Name
                    Species = p.Species,  // Map Pokémon Species
                    DexNumber = p.DexNumber // Map Pokémon DexNumber
                }).ToList()
            }));

        // Act
        var res = trainerService.GetTeam("Ash");
        var resList = res.ToList();

        // Assert
        Assert.Single(resList); // Only 1 trainer should be returned
        Assert.Equal("Ash", resList[0].Name); // Assert the trainer's name
        Assert.Contains(resList[0].Team, p => p.Name == "Pikachu"); // Assert Pikachu in the team
        Assert.Contains(resList[0].Team, p => p.Name == "Charizard"); // Assert Charizard in the team
    }


    [Fact]
    public void GetTrainerById_Exists_Test()
    {

    }


    [Fact]
    public void CreateNewTrainerDuplicateTest()
    {

    }

    [Fact]
    public void LoginTest()
    {
        // Arrange
        var mockTrainerRepo = new Mock<ITrainerRepository>();
        var mockMapper = new Mock<IMapper>();
        var trainerService = new TrainerService(mockTrainerRepo.Object, mockMapper.Object);

        Trainer t = new Trainer { Id = 0, Name = "Bob", Username = "admin", Password = "password" };

        mockTrainerRepo.Setup(repo => repo.GetTrainerByUsername(t.Username)).Returns(t);

        //Act
        var result = trainerService.Login(t.Username, t.Password);
        //Assert

        Assert.NotNull(result);
        Assert.Equal(t.Name, result.Name);
        mockTrainerRepo.Verify(x => x.GetTrainerByUsername(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public void LoginTrainerNullTest()
    {
        // Arrange
        var mockTrainerRepo = new Mock<ITrainerRepository>();
        var mockMapper = new Mock<IMapper>();
        var trainerService = new TrainerService(mockTrainerRepo.Object, mockMapper.Object);

        Trainer t = new Trainer { Id = 0, Name = "Bob", Username = "admin", Password = "password" };
        Trainer test = null;
        mockTrainerRepo.Setup(repo => repo.GetTrainerByUsername(t.Username)).Returns(test);

        //Act
        Action result = () => trainerService.Login(t.Username, t.Password);
        Exception exception = Assert.Throws<Exception>(result);

        //Assert
        Assert.Equal("This trainer doesn't exist", exception.Message);
        mockTrainerRepo.Verify(x => x.GetTrainerByUsername(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public void LoginPasswordMatchTest()
    {

    }

    [Fact]
    public void UpdateTrainerTest()
    {

    }


}