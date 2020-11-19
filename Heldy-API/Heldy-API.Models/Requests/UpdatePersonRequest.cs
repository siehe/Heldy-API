using System;
using System.Collections.Generic;
using System.Text;

namespace Heldy.Models.Requests
{
    public class UpdatePersonRequest
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string SecondName { get; set; }

        public DateTime? DOB { get; set; }
    }
}
