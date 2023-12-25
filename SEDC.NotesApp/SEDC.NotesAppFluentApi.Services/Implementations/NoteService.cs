using SEDC.NotesAppFluentApi.DataAccess.Interfaces;
using SEDC.NotesAppFluentApi.Domain.Models;
using SEDC.NotesAppFluentApi.DTOs.Notes;
using SEDC.NotesAppFluentApi.Mappers.Notes;
using SEDC.NotesAppFluentApi.Services.Interfaces;
using SEDC.NotesAppFluentApi.Shared;
using SEDC.NotesAppFluentAPi.Shared.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvalidDataException = SEDC.NotesAppFluentAPi.Shared.Shared.InvalidDataException;

namespace SEDC.NotesAppFluentApi.Services.Implementations
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly IUserRepository _userRepository;

        public NoteService(IRepository<Note> noteRepository, IUserRepository userRepository) //Dependency Injection
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }

        public List<NotesDto> GetAll()
        {
            List<Note> notesDb = _noteRepository.GetAll();

            return notesDb.Select(x => x.ToNoteDto()).ToList();
        }

        public NotesDto GetById(int id)
        {

            Note note = _noteRepository.GetById(id);
            if(note == null)
            {
                throw new ResourceNotFoundException($"Note with id {id} was not found");
            }
            return note.ToNoteDto();
        }

        public void AddNote(AddNoteDto addNoteDto)
        {
            if(addNoteDto == null)
            {
                throw new Exception("cannot add");
            }


            ValidationHelper.ValidateRequiredStringColumn(addNoteDto.Text, "Text", 100);
            User userDb = _userRepository.GetById(addNoteDto.UserId);

            if(userDb == null)
            {
                throw new InvalidDataException("User not found");
            }

            //From addNoteDto we need to get Note object
            Note note = addNoteDto.ToNote();

            //Note object to repository
            _noteRepository.Add(note);
        }

        public void UpdateNote(UpdateNoteDto updateNoteDto)
        {
            //validate that the note for update exists
            Note noteDb = _noteRepository.GetById(updateNoteDto.Id);
            if( noteDb == null)
            {
                throw new ResourceNotFoundException($"Note with id {updateNoteDto.Id} was not found");
            }

            //2. validate incoming data
             if (string.IsNullOrEmpty(updateNoteDto.Text))
            {
                throw new InvalidDataException("Test must be provided");
            }
            User userDb = _userRepository.GetById(updateNoteDto.UserId);

            if(userDb == null)
            {
                throw new InvalidDataException("User not found");
            }
            //3. edit data
            //we always update the record that is alread from db
            noteDb.Text = updateNoteDto.Text;
            noteDb.Priority = updateNoteDto.Priority;
            noteDb.Tag = updateNoteDto.Tag;
            noteDb.UserId = updateNoteDto.UserId;
            noteDb.User = userDb;
            //4. update in db
            _noteRepository.Update(noteDb);
        }

        public void DeleteNote(int id)
        {
            //1. validate and get the note 
            Note noteDb = _noteRepository.GetById(id);
            if (noteDb == null)
            {
                throw new ResourceNotFoundException($"Note with id {id} was not found");
            }

            //2.Delete the note
            _noteRepository.Delete(noteDb);
        }
    }
}
