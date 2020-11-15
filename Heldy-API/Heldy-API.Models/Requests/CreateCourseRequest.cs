using System;
using System.Collections.Generic;
using System.Text;

namespace Heldy.Models.Requests
{
    public class CreateCourseRequest
    {
        public string Title { get; set; }

        public int Credits { get; set; }

        public IEnumerable<CreateUpdateTaskRequest> Tasks { get; set; }
    }
}
