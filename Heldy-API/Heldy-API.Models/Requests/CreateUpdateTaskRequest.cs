using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Heldy.Models.Requests
{
    public class CreateUpdateTaskRequest
    {
        public int Id { get; set; }

        [MinLength(3), MaxLength(50)]
        public string Statement { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int AssigneeId { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public int StatusId { get; set; }

        public bool IsInQa { get; set; }
    }
}
