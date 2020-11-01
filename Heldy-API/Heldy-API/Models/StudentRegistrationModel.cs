namespace Heldy_API.Models
{
    public class StudentRegistrationModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}