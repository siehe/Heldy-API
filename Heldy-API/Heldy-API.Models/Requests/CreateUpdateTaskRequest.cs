using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heldy.Models.Requests
{
    public class CreateUpdateTaskRequest
    {
        public string Statement { get; set; }

        public DateTime Deadline { get; set; }

        public string Description { get; set; }

        public int SubjectId { get; set; }

        public int AssigneeId { get; set; }

        public int AuthorId { get; set; }

        public int TypeId { get; set; }

        public int StatusId { get; set; }
    }
}
