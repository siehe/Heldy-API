using System;
using System.Collections.Generic;
using System.Text;

namespace Heldy.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string SecondName { get; set; }

        public DateTime DOB { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
