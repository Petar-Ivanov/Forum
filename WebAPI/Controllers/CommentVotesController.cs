using ApplicationServices.Interfaces;
using Messaging.Responses.Get;
using Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Messaging.Requests.Models;
using Messaging.Responses.Create;
using Messaging.Responses.Delete;
using Messaging.Responses.Update;
using Messaging.Responses.GetBy;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing the votes of comments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommentVotesController : ControllerBase
    {
        private readonly ICommentVoteManagementService _commentVoteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentVotesController"/> class.
        /// </summary>
        /// <param name="commentVoteService">The comment vote management service.</param>
        public CommentVotesController(ICommentVoteManagementService commentVoteService)
        {
            _commentVoteService = commentVoteService;
        }

        /// <summary>
        /// Retrieves all comment votes.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/CommentVotes
        /// Vote model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>IsPositive</b>: bool</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>CommentId</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <returns>A list of comment votes.</returns>
        /// <response code="200">Returns the list of comment votes.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetCommentVotesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() => Ok(await _commentVoteService.GetVotes());
        //...
        [Authorize]
        [HttpGet("{discussion_id}/{user_id}/{comment_id}")]
        [ProducesResponseType(typeof(GetCommentVoteByIdsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] int discussion_id, int user_id, int comment_id) => Ok(await _commentVoteService.GetVoteByIds(new(discussion_id,user_id,comment_id)));

        /// <summary>
        /// Creates a new comment vote.
        /// </summary>
        /// <remarks>
        /// Parameters <b>id</b> and <b>source</b> can be null. 
        ///  
        /// The <b>commentId</b> and <b>userId</b> pair must be unique
        /// 
        /// Sample request:
        ///
        ///     POST api/CommentVotes
        ///     {
        ///         "id": null,
        ///         "isPositive": true,
        ///         "source": "Google Chrome",
        ///         "discussionId": 5,
        ///         "userId": 7,
        ///         "commentId": 5
        ///     }
        /// Vote model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>IsPositive</b>: bool</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>CommentId</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="commentVote">The comment vote to create and wether it is positive or negative.</param>
        /// <returns>The created comment vote.</returns>
        /// <response code="200">Returns the created comment vote.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateVoteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] VoteModel commentVote) => Ok(await _commentVoteService.CreateVote(new(commentVote)));

        /// <summary>
        /// Deletes a comment vote by ID.
        /// </summary>
        /// <remarks>
        /// Parameter <b>id</b> cannot be null. 
        /// 
        /// Sample request:
        ///
        ///     DELETE api/CommentVotes/45
        /// Vote model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>IsPositive</b>: bool</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>CommentId</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="id">The ID of the comment vote to delete.</param>
        /// <returns>A result indicating whether the deletion was successful.</returns>
        /// <response code="200">If the comment vote was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteVoteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id) => Ok(await _commentVoteService.DeleteVote(new(id)));

        /// <summary>
        /// Updates an existing comment vote.
        /// </summary>
        /// <remarks>
        /// Parameter <b>source</b> can be null. 
        ///  
        /// The <b>commentId</b> and <b>userId</b> pair must be unique
        /// 
        /// Any values different than those of the chosen comment vote by <b>id</b> will replace the original.
        /// 
        /// Sample request:
        ///
        ///     PUT api/CommentVotes
        ///     {
        ///         "id": 29,
        ///         "isPositive": false,
        ///         "source": "Google Chrome",
        ///         "discussionId": 5,
        ///         "userId": 7,
        ///         "commentId": 5
        ///     }
        /// Vote model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>IsPositive</b>: bool</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>CommentId</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="commentVote">The comment vote to update.</param>
        /// <returns>The updated comment vote.</returns>
        /// <response code="200">Returns the updated comment vote.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateCommentVoteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] VoteModel commentVote) => Ok(await _commentVoteService.UpdateVote(new(commentVote)));
    }
}
