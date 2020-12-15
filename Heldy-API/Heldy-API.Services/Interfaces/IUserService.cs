using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Heldy.Models;
using Heldy.Models.Requests;
using Heldy.Services.DTO;

namespace Heldy.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> Registration(Person user);
        Task<LoginResultDto> Login(Person user);
        Task<string> RegisterStudent(string email);
        Task<Person> GetPerson(int id);
        Task UpdatePersonAsync(UpdatePersonRequest updatePersonRequest, int personId);
        Task UpdatePersonImage(int personId, string fileName);
    }
}
