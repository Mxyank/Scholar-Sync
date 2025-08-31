using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scholarship_Plaatform_Backend.Exceptions
{
    public class ScholarshipException : Exception
    {
        public ScholarshipException(string message) : base(message)
        {

        }
    }
}
