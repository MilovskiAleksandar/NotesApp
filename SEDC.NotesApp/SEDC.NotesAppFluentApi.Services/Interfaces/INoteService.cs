using SEDC.NotesAppFluentApi.Domain.Models;
using SEDC.NotesAppFluentApi.DTOs.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.Services.Interfaces
{
    public interface INoteService
    {
        List<NotesDto> GetAll();
        NotesDto GetById(int id);
        void AddNote(AddNoteDto addNoteDto);
        void DeleteNote(int id);
        void UpdateNote(UpdateNoteDto updateNoteDto);
    }
}
