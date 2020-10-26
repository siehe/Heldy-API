using Heldy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<Subject>> GetSubjectsAsync();
    }
}
