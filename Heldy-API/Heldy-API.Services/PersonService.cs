using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services
{
    public class PersonService : IPersonService
    {
        private IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        
        public async Task<IEnumerable<Person>> GetPersonsAsync(int roleId)
        {
            var persons = await _personRepository.GetPersonsAsync(roleId);
            return persons;
        }
    }
}
