using AutoMapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using PokemonTracker.API.DTO;
using PokemonTracker.API.Migrations;
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
    public void GetTrainerById_EmptyTeam_Test()
    {
        // Arrange
        var mockTrainerRepo = new Mock<ITrainerRepository>();
        var mockMapper = new Mock<IMapper>();
        var trainerService = new TrainerService(mockTrainerRepo.Object, mockMapper.Object);

        // mock trainer with no team
        var trainer = new Trainer
        {
            Id = 1,
            Name = "Ash"
        };

        // Mock the repo's get trainer by id
        mockTrainerRepo.Setup(repo => repo.GetTrainerById(1)).Returns(trainer);
        // Mock the GetTeam method to return an empty list 
        mockTrainerRepo.Setup(repo => repo.GetTeam("Ash")).Returns(new List<Trainer>());  

        // Act
        var res = trainerService.GetTrainerById(1);

        // Assert
        Assert.NotNull(res);               
        Assert.Equal(1, res.Id);           // id's match
        Assert.Equal("Ash", res.Name);     
        Assert.Empty(res.Team);            
    }


    [Fact]
    public void GetTrainerById_TeamExists_Test()
    {
        // Arrange
        var mockTrainerRepo = new Mock<ITrainerRepository>();

        // Configure AutoMapper manually
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();  // Add the profile
        });
        IMapper mapper = config.CreateMapper(); // Create the Mapper instance

        var trainerService = new TrainerService(mockTrainerRepo.Object, mapper);

        // mock trainer
        var trainer = new Trainer
        {
            Id = 1,
            Name = "Ash"
        };

        // Mock the repo's get trainer by id
        mockTrainerRepo.Setup(repo => repo.GetTrainerById(1)).Returns(trainer);
        // Mock the GetTeam method to add the 2 pokemon.
        mockTrainerRepo.Setup(repo => repo.GetTeam("Ash")).Returns(new List<Trainer>
        {
            new Trainer
            {
                Name = "Ash",
                Team = new List<Pkmn>
                {
                    new Pkmn { Name = "Pikachu", Species = "Electric", DexNumber = 7 },
                    new Pkmn { Name = "Charizard", Species = "Fire", DexNumber = 17 }
                }
            }
        });

        // Act
        var res = trainerService.GetTrainerById(1);  // The service calls GetTeam internally

        // Assert
        Assert.NotNull(res);               
        Assert.Equal(1, res.Id);           // id's match
        Assert.Equal("Ash", res.Name);     
        Assert.Equal(2, res.Team.Count);   
        Assert.Contains(res.Team, p => p.Name == "Pikachu"); 
        Assert.Contains(res.Team, p => p.Name == "Charizard"); 
    }


    [Fact]
    public void CreateNewTrainerDuplicateTest()
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

        Trainer newTrainer = new Trainer { Name = "Brock" };

        mockTrainerRepo.Setup(repo => repo.GetTrainerByUsername(It.IsAny<string>())).Returns(newTrainer);
        mockTrainerRepo.Setup(repo => repo.CreateNewTrainer(It.IsAny<Trainer>()))
            .Callback(() => trainerList.Add(newTrainer))
            .Returns(newTrainer);

        // Act
        Action dupe = () => trainerService.CreateNewTrainer(mapper.Map<TrainerInDTO>(newTrainer));
        Exception exception = Assert.Throws<Exception>(dupe);

        // Assert
        Assert.Equal("Duplicate Trainer", exception.Message);
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
    public void LoginPasswordsDontMatchTest()
    {
        // Arrange
        var mockTrainerRepo = new Mock<ITrainerRepository>();
        var mockMapper = new Mock<IMapper>();
        var trainerService = new TrainerService(mockTrainerRepo.Object, mockMapper.Object);

        // List o trainers
        var trainerList = new List<Trainer>
        {
            new Trainer { Id = 0, Name = "Bob", Username = "admin", Password = "password" },
            new Trainer { Id = 1, Name = "Boba", Username = "admina", Password = "passworda" }
        };

        string goodUsername = "admin";
        string badPassword = "asdf";

        // Mock the behavior of the repo method:
        mockTrainerRepo.Setup(repo => repo.GetTrainerByUsername(goodUsername))
            .Returns((string username) => trainerList.FirstOrDefault(t => t.Username == goodUsername));

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => trainerService.Login(goodUsername, badPassword));

        // Assert
        Assert.Equal("The password doesn't match", exception.Message);

        // Verify the mock was called to check the username
        mockTrainerRepo.Verify(x => x.GetTrainerByUsername(It.IsAny<string>()), Times.Once());
    }


    [Fact]
    public void UpdateTrainer_DoesntExist_Test()
    {
        // Arrange
        Mock<ITrainerRepository> mockTrainerRepo = new();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        TrainerService trainerService = new(mockTrainerRepo.Object, mockMapper.Object);

        // List o trainers
        var trainerList = new List<Trainer>
        {
            new Trainer { Id = 1, Name = "Bob" },
            new Trainer { Id = 2, Name = "Alice" }
        };

        // main character ghost
        var ghostDTO = new UpdateDTO
        {
            Id = 999,  // This is the ID that doesn't exist
            Name = "Ghost"
        };

        // Mock repo method GetTrainerById, returns Trainer or null
        mockTrainerRepo.Setup(repo => repo.GetTrainerById(It.Is<int>(id => id == ghostDTO.Id)))
            .Returns((int id) => trainerList.FirstOrDefault(t => t.Id == id));

        // Mocking Mapper to convert UpdateDTO to Trainer
        mockMapper.Setup(m => m.Map<Trainer>(It.IsAny<UpdateDTO>()))
            .Returns((UpdateDTO dto) => new Trainer
            {
                Id = dto.Id,
                Name = dto.Name
            });

        // Mocking Mapper to convert Trainer to TrainerOutDTO
        mockMapper.Setup(m => m.Map<TrainerOutDTO>(It.IsAny<Trainer>()))
            .Returns((Trainer t) => new TrainerOutDTO
            {
                Id = t.Id,
                Name = t.Name,
                Team = t.Team.Select(p => new PkmnOutDTO()).ToList() 
            });

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => trainerService.UpdateTrainer(ghostDTO));

        // Assert 
        Assert.Equal("This trainer doesn't exist!", exception.Message);
        mockTrainerRepo.Verify(x => x.GetTrainerById(It.IsAny<int>()), Times.Once());
    }


    [Fact]
    public void UpdateTrainer_Exists_Test()
    {
        // Arrange
        var mockTrainerRepo = new Mock<ITrainerRepository>();
        var mockMapper = new Mock<IMapper>();
        var trainerService = new TrainerService(mockTrainerRepo.Object, mockMapper.Object);

        // List o trainers
        var trainerList = new List<Trainer>
        {
            new Trainer { Id = 1, Name = "Bob" },
            new Trainer { Id = 2, Name = "Alice" }
        };

        // mock updateDTO req body
        var goodDTO = new UpdateDTO
        {
            Id = 1,
            Name = "Hank"  // tryna change to Hank
        };

        // Mock the repo's GetTrainerById to return the trainer with Id = 1
        mockTrainerRepo.Setup(repo => repo.GetTrainerById(1))
            .Returns((int id) => trainerList.Find(t => t.Id == id));  // Returns the trainer Id 1

        // Mock the repo's UpdateTrainer method to return the updated trainer
        mockTrainerRepo.Setup(repo => repo.UpdateTrainer(It.IsAny<Trainer>()))
            .Returns((Trainer t) => t);  

        // Mocking Mapper to convert UpdateDTO to Trainer
        mockMapper.Setup(m => m.Map<Trainer>(It.IsAny<UpdateDTO>()))
            .Returns((UpdateDTO dto) => new Trainer
            {
                Id = dto.Id,
                Name = dto.Name,
                Team = new List<Pkmn>()  // Initialize team as empty, since we're only testing Name and Id
            });

        // Mocking Mapper to convert Trainer to TrainerOutDTO
        mockMapper.Setup(m => m.Map<TrainerOutDTO>(It.IsAny<Trainer>()))
            .Returns((Trainer t) => new TrainerOutDTO
            {
                Id = t.Id,
                Name = t.Name,
                Team = new List<PkmnOutDTO>()  // Empty team, since we're ignoring the team in this test
            });

        // Act
        var resTrainer = trainerService.UpdateTrainer(goodDTO);  // this is a TrainerOutDTO

        // Assert
        var updatedTrainer = trainerList.FirstOrDefault(t => t.Id == goodDTO.Id);  // Trainer type
        Assert.NotNull(updatedTrainer);  // trainer exists in list
        Assert.Equal(goodDTO.Name, updatedTrainer.Name);  // compare update dto new name to new Trainer name
        mockTrainerRepo.Verify(x => x.GetTrainerById(It.IsAny<int>()), Times.Once());  // Verify GetTrainerById was called once
        mockTrainerRepo.Verify(x => x.UpdateTrainer(It.IsAny<Trainer>()), Times.Once());  // Verify UpdateTrainer was called once
    }
}