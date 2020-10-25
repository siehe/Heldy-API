using Heldy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPersonsAsync(int roleId);
    }
}