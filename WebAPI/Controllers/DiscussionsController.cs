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
using Messaging.Responses.GetBy;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing discussions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionsController : ControllerBase
    {
        private readonly IDiscussionManagementService _discussionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscussionsController"/> class.
        /// </summary>
        /// <param name="discussionService">The discussion management service.</param>
        public DiscussionsController(IDiscussionManagementService discussionService)
        {
            _discussionService = discussionService;
        }

        /// <summary>
        /// Retrieves all discussions.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Discussions
        /// Discussion model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Title</b>: string</li>
        ///     <li><b>Text</b>: string</li>
        ///     <li><b>IsUpdated</b>: bool</li>
        ///     <li><b>IsLocked</b>: bool</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>Image</b>: byte[]?</li>
        ///     <li><b>CreatedBy</b>: int?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <returns>A list of discussions.</returns>
        /// <response code="200">Returns the list of discussions.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(GetDiscussionsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() => Ok(await _discussionService.GetDiscussions());

        // ...
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetDiscussionByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] int id) => Ok(await _discussionService.GetDiscussionById(new(id)));

        // ...
        [HttpGet("title/{title}")]
        [ProducesResponseType(typeof(GetDiscussionsByTitleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByTitle([FromRoute] string title) => Ok(await _discussionService.GetDiscussionsByTitle(new(title)));
        // ...
        [HttpGet("topic/{topic}")]
        [ProducesResponseType(typeof(GetDiscussionsByTopicResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByTopic([FromRoute] string topic) => Ok(await _discussionService.GetDiscussionsByTopic(new(topic)));

        /// <summary>
        /// Creates a new discussion.
        /// </summary>
        /// <remarks>
        /// Parameters <b>id</b>, <b>isVisible</b>, <b>image</b>, <b>createdBy</b> and <b>updatedBy</b> can be null. 
        ///  
        /// The <b>title</b> parameter must be unique
        /// 
        /// Sample request:
        ///
        ///     POST api/Discussions
        ///     {
        ///         "id": null,
        ///         "title": "Some unique title",
        ///         "text": "Sample discussion text",
        ///         "isUpdated": false,
        ///         "isLocked": false,
        ///         "isVisible": true,
        ///         "image": null,
        ///         "createdBy": 7,
        ///         "updatedBy": 6
        ///     }
        /// Discussion model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Title</b>: string</li>
        ///     <li><b>Text</b>: string</li>
        ///     <li><b>IsUpdated</b>: bool</li>
        ///     <li><b>IsLocked</b>: bool</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>Image</b>: byte[]?</li>
        ///     <li><b>CreatedBy</b>: int?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="discussion">The discussion to create.</param>
        /// <returns>The created discussion.</returns>
        /// <response code="200">Returns the created discussion.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateDiscussionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] DiscussionModel discussion) => Ok(await _discussionService.CreateDiscussion(new(discussion)));

        /// <summary>
        /// Deletes a discussion by ID.
        /// </summary>
        /// <remarks>
        /// Parameter <b>id</b> cannot be null. 
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Discussions/45
        /// Discussion model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Title</b>: string</li>
        ///     <li><b>Text</b>: string</li>
        ///     <li><b>IsUpdated</b>: bool</li>
        ///     <li><b>IsLocked</b>: bool</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>Image</b>: byte[]?</li>
        ///     <li><b>CreatedBy</b>: int?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="id">The ID of the discussion to delete.</param>
        /// <returns>A result indicating whether the deletion was successful.</returns>
        /// <response code="200">If the discussion was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteDiscussionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id) => Ok(await _discussionService.DeleteDiscussion(new(id)));

        /// <summary>
        /// Updates an existing discussion.
        /// </summary>
        /// <remarks>
        /// Parameters <b>isVisible</b>, <b>image</b>, <b>createdBy</b> and <b>updatedBy</b> can be null. 
        ///  
        /// The <b>title</b> parameter must be unique
        /// 
        /// Any values different than those of the chosen discussion by <b>id</b> will replace the original.
        /// 
        /// Sample request:
        ///
        ///     PUT api/Discussions
        ///     {
        ///         "id": 13,
        ///         "title": "Some new unique title",
        ///         "text": "Sample discussion text",
        ///         "isUpdated": true,
        ///         "isLocked": false,
        ///         "isVisible": true,
        ///         "image": null,
        ///         "createdBy": 7,
        ///         "updatedBy": 6
        ///     }
        /// Discussion model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Title</b>: string</li>
        ///     <li><b>Text</b>: string</li>
        ///     <li><b>IsUpdated</b>: bool</li>
        ///     <li><b>IsLocked</b>: bool</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>Image</b>: byte[]?</li>
        ///     <li><b>CreatedBy</b>: int?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="discussion">The discussion to update.</param>
        /// <returns>The updated discussion.</returns>
        /// <response code="200">Returns the updated discussion.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateDiscussionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] DiscussionModel discussion) => Ok(await _discussionService.UpdateDiscussion(new(discussion)));
    }
}
