using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heldy.Models.Requests;
using Heldy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Heldy_API.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentsController : Controller
    {
        private ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentRequest createCommentRequest)
        {
            await _commentsService.CreateCommentAsync(createCommentRequest);
            return Ok();
        }

        [HttpGet]
        [Route("{postId}")]
        public async Task<IActionResult> GetCommentsByPost(int postId)
        {
            var comments = await _commentsService.GetCommentsByPostId(postId);

            return Ok(comments);
        }
    }
}
