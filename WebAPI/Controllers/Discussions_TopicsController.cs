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
    /// Controller for managing discussion-topic relationships.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Discussions_TopicsController : ControllerBase
    {
        private readonly IDiscussions_TopicsManagementService _discussions_topicsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Discussions_TopicsController"/> class.
        /// </summary>
        /// <param name="discussions_topicsService">The discussions_topics management service.</param>
        public Discussions_TopicsController(IDiscussions_TopicsManagementService discussions_topicsService)
        {
            _discussions_topicsService = discussions_topicsService;
        }

        /// <summary>
        /// Retrieves all discussion-topic relationships.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Discussions_Topics
        /// Discussions_Topics model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>TopicId</b>: int</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        /// </ul>
        /// </remarks>
        /// <returns>A list of discussion-topic relationships.</returns>
        /// <response code="200">Returns the list of discussion-topic relationships.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(GetDiscussions_TopicsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() => Ok(await _discussions_topicsService.GetDiscussions_Topics());
        //...
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetDiscussions_TopicsByDiscussionIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] int id) => Ok(await _discussions_topicsService.GetDiscussions_TopicsByDiscussionId(new(id)));

        /// <summary>
        /// Creates a new discussion-topic relationships.
        /// </summary>
        /// <remarks>
        /// Parameters <b>id</b> and <b>source</b> can be null. 
        ///  
        /// The <b>discussionId</b> and <b>topicId</b> pair must be unique
        /// 
        /// Sample request:
        ///
        ///     POST api/Discussions_Topics
        ///     {
        ///         "id": null,
        ///         "source": "Firefox",
        ///         "discussionId": 7,
        ///         "topicId": 9,
        ///         "isVisible": true
        ///     }
        /// Discussions_Topics model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>TopicId</b>: int</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        /// </ul>
        /// </remarks>
        /// <param name="discussions_topics">The discussion-topic relationship to create.</param>
        /// <returns>The created discussion-topic relationship.</returns>
        /// <response code="200">Returns the created discussion-topic relationships.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateDiscussions_TopicsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] Discussions_TopicsModel discussions_topics) => Ok(await _discussions_topicsService.CreateDiscussions_Topics(new(discussions_topics)));

        /// <summary>
        /// Deletes a discussion-topic relationship by ID.
        /// </summary>
        /// <remarks>
        /// Parameter <b>id</b> cannot be null. 
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Discussions_Topics/45
        /// Discussions_Topics model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>TopicId</b>: int</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        /// </ul>
        /// </remarks>
        /// <param name="id">The ID of the discussion-topic relationship to delete.</param>
        /// <returns>A result indicating whether the deletion was successful.</returns>
        /// <response code="200">If the discussion-topic relationships was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteDiscussions_TopicsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id) => Ok(await _discussions_topicsService.DeleteDiscussions_Topics(new(id)));
        //...
        [Authorize]
        [HttpDelete("{discussionId}/{topicId}")]
        [ProducesResponseType(typeof(DeleteDiscussions_TopicsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int discussionId, int topicId) => Ok(await _discussions_topicsService.DeleteDiscussions_TopicsByIds(new(discussionId, topicId)));
        /// <summary>
        /// Updates an existing discussion-topic relationship.
        /// </summary>
        /// <remarks>
        /// Parameter <b>source</b> can be null. 
        ///  
        /// The <b>discussionId</b> and <b>topicId</b> pair must be unique
        /// 
        /// Any values different than those of the chosen discussions_topics by <b>id</b> will replace the original.
        /// 
        /// Sample request:
        ///
        ///     PUT api/Discussions_Topics
        ///     {
        ///         "id": 11,
        ///         "source": "Firefox",
        ///         "discussionId": 7,
        ///         "topicId": 9,
        ///         "isVisible": true
        ///     }
        /// Discussions_Topics model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>TopicId</b>: int</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        /// </ul>
        /// </remarks>
        /// <param name="discussions_topics">The discussion-topic relationship to update.</param>
        /// <returns>The updated discussion-topic relationship.</returns>
        /// <response code="200">Returns the updated discussion-topic relationship.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateDiscussions_TopicsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] Discussions_TopicsModel discussions_topics) => Ok(await _discussions_topicsService.UpdateDiscussions_Topics(new(discussions_topics)));
    }
}
