using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Heldy.Models;

namespace Heldy.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task CreateNewPerson(Person person);

        Task<Person> GetPersonByEmail(string email);
    }
}
