﻿using Heldy.Models;
using Heldy.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services.Interfaces
{
    public interface ICommentsService
    {
        Task CreateCommentAsync(CreateCommentRequest createCommentRequest);

        Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);
    }
}
