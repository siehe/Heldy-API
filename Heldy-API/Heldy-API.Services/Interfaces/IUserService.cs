using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Heldy.Models;
using Heldy.Services.DTO;

namespace Heldy.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> Registration(Person user);
        Task<LoginResultDto> Login(Person user);
        Task<string> RegisterStudent(string email);
        Task<Person> GetPerson(int id);
    }
}
