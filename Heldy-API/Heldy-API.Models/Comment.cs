using System;
using System.Collections.Generic;
using System.Text;

namespace Heldy.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public bool IsForTicket { get; set; }

        public int ReplyTo { get; set; }

        public string Text { get; set; }

        public DateTime? PostDate { get; set; }
    }
}
