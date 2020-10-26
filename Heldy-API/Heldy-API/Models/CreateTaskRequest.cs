using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heldy_API.Models
{
    public class CreateTaskRequest
    {
        public string Statement { get; set; }

        public DateTime Deadline { get; set; }

        public string Description { get; set; }

        public int SubjectId { get; set; }

        public int AssigneId { get; set; }

        public int AuthorId { get; set; }

        public int TypeId { get; set; }

        public int StatusId { get; set; }
    }
}
