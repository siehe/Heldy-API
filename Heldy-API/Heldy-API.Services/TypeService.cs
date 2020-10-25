using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services
{
    public class TypeService : ITypeService
    {
        private ITypeRepository _typeRepository;

        public TypeService(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<IEnumerable<TaskType>> GetTypesAsync()
        {
            var types = await _typeRepository.GetTypesAsync();
            return types;
        }
    }
}
