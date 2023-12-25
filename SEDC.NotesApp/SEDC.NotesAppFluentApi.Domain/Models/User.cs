using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.Domain.Models
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
        public int Age { get; set; }

        public List<Note> Notes { get; set; }
    }
}
