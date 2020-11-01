using System;
using System.ComponentModel.DataAnnotations;

namespace Heldy_API.Models
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }

        public string Surname { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}