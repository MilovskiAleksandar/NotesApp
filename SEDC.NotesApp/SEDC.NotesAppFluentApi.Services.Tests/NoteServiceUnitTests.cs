using Moq;
using SEDC.NotesAppFluentApi.DataAccess.Interfaces;
using SEDC.NotesAppFluentApi.Domain.Models;
using SEDC.NotesAppFluentApi.DTOs.Notes;
using SEDC.NotesAppFluentApi.Services.Implementations;
using SEDC.NotesAppFluentApi.Services.Interfaces;
using SEDC.NotesAppFluentAPi.Shared.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.Services.Tests
{
    [TestClass]
    public class NoteServiceUnitTests
    {
        private readonly INoteService _noteService;
        private readonly Mock<IRepository<Note>> _noteRepository;
        private readonly Mock<IUserRepository> _userRepository;

        public NoteServiceUnitTests()
        {
             _noteRepository = new Mock<IRepository<Note>>();
             _userRepository = new Mock<IUserRepository>();

            _noteService = new NoteService(_noteRepository.Object, _userRepository.Object);
        }

        [TestMethod]
        public void GetAll_should_return_NotesDtos()
        {
            //Arrange
            List<Note> notes = new List<Note>()
            {
                new Note
                {
                    Id = 1,
                    Priority = Domain.Enums.Priority.High,
                    Tag = Domain.Enums.Tag.Work,
                    Text = "Do the homework",
                    UserId = 1,
                    User = new User
                    {
                        Id = 1,
                        FirstName = "Test1",
                        LastName = "User1"
                    }
                },

                new Note
                {
                    Id = 2,
                    Priority = Domain.Enums.Priority.Medium,
                    Tag = Domain.Enums.Tag.Health,
                    Text = "Drink water",
                    UserId = 2,
                    User = new User
                    {
                        Id = 2,
                        FirstName = "Test2",
                        LastName = "User2"
                    }
                }
            };
            _noteRepository.Setup(x => x.GetAll()).Returns(notes);
            //Act
            List<NotesDto> resultNotes = _noteService.GetAll();

            //Assert
            Assert.AreEqual(2, resultNotes.Count);
            Assert.AreEqual("Do the homework", resultNotes.First().Text);
            Assert.AreEqual("Drink water", resultNotes.Last().Text);
            Assert.AreEqual("Test1 User1", resultNotes.First().UserFullName);
            Assert.AreEqual("Test2 User2", resultNotes.Last().UserFullName);

        }

        [TestMethod]
        public void GetAll_should_return_emptyList_On_EmptyList_Fom_Db()
        {
            //Arrange
            //simulate that no data was returned from db
            _noteRepository.Setup(x => x.GetAll()).Returns(new List<Note>());

            //Act
            List<NotesDto> resultNotes = _noteService.GetAll();

            //Asset
            Assert.AreEqual(0, resultNotes.Count);
        }

        [TestMethod]
        public void GetById_Should_Throw_On_NoteNotFound()
        {
            //Arrange
            int id = 2;
            _noteRepository.Setup(x => x.GetById(id)).Returns(null as Note);

            //Act and Assert

            var exception = Assert.ThrowsException<ResourceNotFoundException>(() =>  _noteService.GetById(id));

            //Assert
            Assert.AreEqual($"Note with id {id} was not found", exception.Message);
        }
    }
}
