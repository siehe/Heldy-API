using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Heldy.Models;
using Heldy.Models.Requests;

namespace Heldy.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task CreateNewTeacher(Person person);

        Task CreateNewStudent(Person person);

        Task<Person> GetPersonByEmail(string email);

        Task<Person> GetPerson(int id);

        Task UpdatePersonAsync(UpdatePersonRequest updatePersonRequest, int personId);

        Task UpdatePersonImage(int personId, string fileName);
    }
}
