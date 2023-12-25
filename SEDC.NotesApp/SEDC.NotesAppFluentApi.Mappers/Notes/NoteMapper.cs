using SEDC.NotesAppFluentApi.Domain.Models;
using SEDC.NotesAppFluentApi.DTOs.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.Mappers.Notes
{
    public static class NoteMapper
    {
        public static NotesDto ToNoteDto(this Note note)
        {
            return new NotesDto
            {
                Priority = (int)note.Priority,
                Tag = (int)note.Tag,
                Text = note.Text,
                UserFullName = $"{note.User.FirstName} {note.User.LastName}"
            };
        }

        public static Note ToNote(this AddNoteDto addNoteDto)
        {
            return new Note
            {
                Text = addNoteDto.Text,
                Priority = addNoteDto.Priority,
                Tag = addNoteDto.Tag,
                UserId = addNoteDto.UserId
            };
        }
    }
}
