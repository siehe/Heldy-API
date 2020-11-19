using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Models.Requests;
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
        private IUserRepository _userRepository;

        public PersonService(IPersonRepository personRepository, IUserRepository userRepository)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
        }
        
        public async Task<IEnumerable<Person>> GetPersonsAsync(int roleId)
        {
            var persons = await _personRepository.GetPersonsAsync(roleId);
            return persons;
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            var person = await _personRepository.GetPersonAsync(id);
            return person;
        }
    }
}
