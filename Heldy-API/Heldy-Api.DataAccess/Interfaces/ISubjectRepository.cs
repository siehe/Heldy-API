using Heldy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.DataAccess.Interfaces
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetSubjectsAsync();

        Task<int> CreateSubjectAsync(Subject subject);
    }
}
