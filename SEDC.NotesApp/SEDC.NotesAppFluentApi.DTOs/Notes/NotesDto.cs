using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.DTOs.Notes
{
    public class NotesDto
    {
        public string Text { get; set; }
        public int Priority { get; set; }
        public int Tag { get; set; }
        public string UserFullName { get; set; } //this is why we do a join(Include) wutg the ysers table to be able to get UserFullName.
    }
}
