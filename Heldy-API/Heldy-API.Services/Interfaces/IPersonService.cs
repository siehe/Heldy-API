using Heldy.Models;
using Heldy.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPersonsAsync(int roleId);

        Task<Person> GetPersonAsync(int id);
    }
}