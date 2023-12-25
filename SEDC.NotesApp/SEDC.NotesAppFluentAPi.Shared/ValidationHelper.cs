using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDC.NotesAppFluentAPi.Shared;

namespace SEDC.NotesAppFluentApi.Shared
{
    public class ValidationHelper
    {
        public static void ValidateRequiredStringColumn(string value,string field, int maxNumOfChars)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidDataException($"{field} must be provided");
            }

            if(value.Length > maxNumOfChars)
            {
                throw new InvalidDataException($"{field} can not contain more {maxNumOfChars} characters");
            }
        }

        public static void ValidateStringColumnLenght(string value, string field, int maxNumOfChars)
        {
            if (value.Length > maxNumOfChars)
            {
                throw new InvalidDataException($"{field} can not contain more {maxNumOfChars} characters");
            }
        }
    }
}
