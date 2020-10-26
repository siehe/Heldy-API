using Heldy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.DataAccess.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPersonsAsync(int roleId);
    }
}
