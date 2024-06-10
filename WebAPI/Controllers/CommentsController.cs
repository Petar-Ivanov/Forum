using ApplicationServices.Interfaces;
using Messaging;
using Messaging.Requests.Models;
using Messaging.Responses.Create;
using Messaging.Responses.Delete;
using Messaging.Responses.Get;
using Messaging.Responses.GetBy;
using Messaging.Responses.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing comments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentManagementService _commentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="commentService">The comment management service.</param>
        public CommentsController(ICommentManagementService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Retrieves all comments.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Comments
        /// 
        /// Comment model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>IsUpdated</b>: bool</li>
        ///     <li><b>Text</b>: string</li>
        /// </ul>
        /// </remarks>
        /// <returns>A list of comments.</returns>
        /// <response code="200">Returns the list of comments.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetCommentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() => Ok(await _commentService.GetComments());
        //public async Task<GetCommentsByIdResponse> GetCommentById(GetCommentsByIdRequest request)
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetCommentByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id) => Ok(await _commentService.GetCommentsById(new(id)));

        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(GetCommentsResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetSingle(int id) => Ok(await _commentService.GetCommentById(new(id)));

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <remarks>
        /// Parameters <b>id</b> and <b>source</b> can be null. 
        ///  
        /// The <b>discussionId</b> and <b>userId</b> pair must be unique
        /// 
        /// Sample request:
        ///
        ///     POST api/Comments
        ///     {
        ///        "id": null,
        ///        "source": "Google Chrome",
        ///        "discussionId": 2,
        ///        "userId": 6,
        ///        "isUpdated": false,
        ///        "text": "Sample comment text."
        ///     }
        /// Comment model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>IsUpdated</b>: bool</li>
        ///     <li><b>Text</b>: string</li>
        /// </ul>
        /// </remarks>
        /// <param name="comment">The comment to create.</param>
        /// <returns>The created comment.</returns>
        /// <response code="200">Returns the created comment.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateCommentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CommentModel comment) => Ok(await _commentService.CreateComment(new(comment)));

        /// <summary>
        /// Deletes a comment by ID.
        /// </summary>
        /// <remarks>
        /// Parameter <b>id</b> cannot be null. 
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Comments/45
        /// Comment model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>IsUpdated</b>: bool</li>
        ///     <li><b>Text</b>: string</li>
        /// </ul>
        /// </remarks>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>A result indicating whether the deletion was successful.</returns>
        /// <response code="200">If the comment was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteCommentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id) => Ok(await _commentService.DeleteComment(new(id)));

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <remarks>
        /// Parameter <b>source</b> can be null. 
        ///  
        /// The <b>discussionId</b> and <b>userId</b> pair must be unique
        /// 
        /// Any values different than those of the chosen comment by <b>id</b> will replace the original.
        /// 
        /// Sample request:
        ///
        ///     PUT api/Comments
        ///     {
        ///        "id": 7,
        ///        "source": "Firefox",
        ///        "discussionId": 2,
        ///        "userId": 6,
        ///        "isUpdated": true,
        ///        "text": "Sample comment text."
        ///     }
        /// Comment model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>IsUpdated</b>: bool</li>
        ///     <li><b>Text</b>: string</li>
        /// </ul>
        /// </remarks>
        /// <param name="comment">The comment to update.</param>
        /// <returns>The updated comment.</returns>
        /// <response code="200">Returns the updated comment.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateCommentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] CommentModel comment) => Ok(await _commentService.UpdateComment(new(comment)));
    }
}
