using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services
{
    public class ColumnService : IColumnService
    {
        private IColumnsRepository _columnRepository;

        public ColumnService(IColumnsRepository columnsRepository)
        {
            _columnRepository = columnsRepository;
        }

        public async Task<IEnumerable<Column>> GetColumns()
        {
            var columns = await _columnRepository.GetColumns();
            return columns;
        }
    }
}
