using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Heldy.Models
{
    public class PersonTask
    {
        public int Id { get; set; }

        public string Statement { get; set; }

        public string Description { get; set; }

        public TaskType Type { get; set; }

        public DateTime Deadline { get; set; }

        public Subject Subejct { get; set; }

        public Person Author { get; set; }

        public Person Assignee { get; set; }

        public Column Status { get; set; }

        public string EctsMark { get; set; }

        public int Grade { get; set; }

        public string Comment { get; set; }
    }
}
