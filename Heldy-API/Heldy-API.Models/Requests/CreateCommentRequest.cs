using System;
using System.Collections.Generic;
using System.Text;

namespace Heldy.Models.Requests
{
    public class CreateCommentRequest
    {
        public int AuthorId { get; set; }

        public bool IsForTicket { get; set; }

        public int ReplyTo { get; set; }

        public string Text { get; set; }
    }
}
