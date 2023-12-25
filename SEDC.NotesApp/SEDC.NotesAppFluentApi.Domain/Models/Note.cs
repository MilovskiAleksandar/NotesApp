using SEDC.NotesAppFluentApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.Domain.Models
{
    public class Note : BaseEntity
    {
        public string Text { get; set; }
        public Priority Priority { get; set; }

        public Tag Tag { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
