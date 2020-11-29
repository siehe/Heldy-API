using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Models.Requests;
using Heldy.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services
{
    public class CommentsService : ICommentsService
    {
        private ICommentsRespository _commentsRepository;

        public CommentsService(ICommentsRespository commentsRespository)
        {
            _commentsRepository = commentsRespository;
        }

        public async Task CreateCommentAsync(CreateCommentRequest createCommentRequest)
        {
            await _commentsRepository.CreateCommentAsync(createCommentRequest);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            var comments = await _commentsRepository.GetCommentsByPostId(postId);

            return comments;
        }
    }
}
