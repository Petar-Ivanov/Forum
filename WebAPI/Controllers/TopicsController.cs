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
    /// Controller for managing topics.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicManagementService _topicService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicsController"/> class.
        /// </summary>
        /// <param name="topicService">The topics management service.</param>
        public TopicsController(ITopicManagementService topicService)
        {
            _topicService = topicService;
        }

        /// <summary>
        /// Retrieves all topics.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Topics
        /// Topic model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Name</b>: string</li>
        ///     <li><b>Description</b>: string</li>
        ///     <li><b>IsVisible</b>: bool</li>
        ///     <li><b>CreatedBy</b>: int?</li>
        ///     <li><b>CreatedOn</b>: DateTime?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <returns>A list of topics.</returns>
        /// <response code="200">Returns the list of topics.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() => Ok(await _topicService.GetTopics());
        // ...
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTopicByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] int id) => Ok(await _topicService.GetTopicById(new(id)));
        // ...
        [HttpGet("name/{name}")]
        [ProducesResponseType(typeof(GetTopicsByNameResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] string name) => Ok(await _topicService.GetTopicsByName(new(name)));

        /// <summary>
        /// Creates a new topic.
        /// </summary>
        /// <remarks>
        /// Parameters <b>id</b>, <b>createdBy</b>, <b>createdOn</b> and <b>updatedBy</b> can be null. 
        ///  
        /// The <b>name</b> parameter must be unique
        /// 
        /// Sample request:
        ///
        ///     POST api/Topics
        ///     {
        ///         "id": null,
        ///         "name": "A unique topic name",
        ///         "description": "A sample topic description.",
        ///         "isVisible": true,
        ///         "createdBy": 7,
        ///         "createdOn": "2024-05-18T22:22:44.557Z",
        ///         "updatedBy": null
        ///     }
        /// Topic model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Name</b>: string</li>
        ///     <li><b>Description</b>: string</li>
        ///     <li><b>IsVisible</b>: bool</li>
        ///     <li><b>CreatedBy</b>: int?</li>
        ///     <li><b>CreatedOn</b>: DateTime?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="topic">The topic to create.</param>
        /// <returns>The created topic.</returns>
        /// <response code="200">Returns the created topic.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateTopicResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] TopicModel topic) => Ok(await _topicService.CreateTopic(new(topic)));

        /// <summary>
        /// Deletes a topic by ID.
        /// </summary>
        /// <remarks>
        /// Parameter <b>id</b> cannot be null. 
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Topics/45
        /// Topic model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Name</b>: string</li>
        ///     <li><b>Description</b>: string</li>
        ///     <li><b>IsVisible</b>: bool</li>
        ///     <li><b>CreatedBy</b>: int?</li>
        ///     <li><b>CreatedOn</b>: DateTime?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="id">The ID of the topic to delete.</param>
        /// <returns>A result indicating whether the deletion was successful.</returns>
        /// <response code="200">If the topic was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteTopicResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id) => Ok(await _topicService.DeleteTopic(new(id)));

        /// <summary>
        /// Updates an existing topic.
        /// </summary>
        /// <remarks>
        /// Parameters <b>createdBy</b> and <b>createdOn</b> can be null. 
        ///  
        /// The <b>name</b> parameter must be unique
        /// 
        /// Any values different than those of the chosen topic by <b>id</b> will replace the original.
        /// 
        /// Sample request:
        ///
        ///     PUT api/Topics
        ///     {
        ///         "id": 8,
        ///         "name": "A changed topic name",
        ///         "description": "A sample topic description.",
        ///         "isVisible": true,
        ///         "createdBy": 7,
        ///         "createdOn": "2024-05-18T22:22:44.557Z",
        ///         "updatedBy": 9
        ///     }
        /// Topic model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Name</b>: string</li>
        ///     <li><b>Description</b>: string</li>
        ///     <li><b>IsVisible</b>: bool</li>
        ///     <li><b>CreatedBy</b>: int?</li>
        ///     <li><b>CreatedOn</b>: DateTime?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="topic">The topic to update.</param>
        /// <returns>The updated topic.</returns>
        /// <response code="200">Returns the updated topic.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateTopicResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] TopicModel topic) => Ok(await _topicService.UpdateTopic(new(topic)));
    }
}
