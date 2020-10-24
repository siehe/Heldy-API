using Heldy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heldy.DataAccess.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<PersonTask>> GetPersonTasksAsync(int userId);
    }
}
