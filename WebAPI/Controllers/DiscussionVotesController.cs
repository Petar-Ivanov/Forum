using ApplicationServices.Interfaces;
using Messaging.Responses.Get;
using Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Messaging.Requests.Models;
using Messaging.Responses.Create;
using Messaging.Requests.Delete;
using Messaging.Responses.Delete;
using Messaging.Responses.Update;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing discussion votes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionVotesController : ControllerBase
    {
        private readonly IDiscussionVoteManagementService _discussionVoteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscussionVotesController"/> class.
        /// </summary>
        /// <param name="discussionVoteService">The discussion votes management service.</param>
        public DiscussionVotesController(IDiscussionVoteManagementService discussionVoteService)
        {
            _discussionVoteService = discussionVoteService;
        }

        /// <summary>
        /// Retrieves all discussion votes.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/DiscussionVotes
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
        /// <returns>A list of discussion votes.</returns>
        /// <response code="200">Returns the list of discussion votes.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(GetDiscussionVotesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() => Ok(await _discussionVoteService.GetVotes());
        //...
        [HttpGet("{discussion_id}/{user_id}")]
        [ProducesResponseType(typeof(GetDiscussionVotesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] int discussion_id, int user_id) => Ok(await _discussionVoteService.GetVoteByIds(new(discussion_id, user_id)));

        /// <summary>
        /// Creates a new discussion vote.
        /// </summary>
        /// <remarks>
        /// Parameters <b>id</b> and <b>source</b> can be null. 
        ///  
        /// The <b>commentId</b> and <b>userId</b> pair must be unique
        /// 
        /// Sample request:
        ///
        ///     POST api/DiscussionVotes
        ///     {
        ///         "id": null,
        ///         "isPositive": true,
        ///         "source": "Safari",
        ///         "discussionId": 7,
        ///         "userId": 45,
        ///         "commentId": 3
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
        /// <param name="discussionVote">The discussion vote to create.</param>
        /// <returns>The created comment.</returns>
        /// <response code="200">Returns the created discussion vote.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateVoteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] VoteModel discussionVote) => Ok(await _discussionVoteService.CreateVote(new(discussionVote)));

        /// <summary>
        /// Deletes a discussion vote by ID.
        /// </summary>
        /// <remarks>
        /// Parameter <b>id</b> cannot be null. 
        /// 
        /// Sample request:
        ///
        ///     DELETE api/DiscussionVotes/45
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
        /// <param name="id">The ID of the discussion vote to delete.</param>
        /// <returns>A result indicating whether the deletion was successful.</returns>
        /// <response code="200">If the discussion vote was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteVoteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id) => Ok(await _discussionVoteService.DeleteVote(new(id)));

        /// <summary>
        /// Updates an existing discussion vote.
        /// </summary>
        /// <remarks>
        /// Parameter <b>source</b> can be null. 
        ///  
        /// The <b>commentId</b> and <b>userId</b> pair must be unique
        /// 
        /// Any values different than those of the chosen discussion votes by <b>id</b> will replace the original.
        /// 
        /// Sample request:
        ///
        ///     PUT api/DiscussionVotes
        ///     {
        ///         "id": 46,
        ///         "isPositive": false,
        ///         "source": "Safari",
        ///         "discussionId": 7,
        ///         "userId": 45,
        ///         "commentId": 3
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
        /// <param name="discussionVote">The discussion vote to update.</param>
        /// <returns>The updated discussion vote.</returns>
        /// <response code="200">Returns the updated discussion vote.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateDiscussionVoteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] VoteModel discussionVote) => Ok(await _discussionVoteService.UpdateVote(new(discussionVote)));
    }
}
