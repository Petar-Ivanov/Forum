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
    /// Controller for managing views.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ViewsController : ControllerBase
    {
        private readonly IViewManagementService _viewService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewsController"/> class.
        /// </summary>
        /// <param name="viewService">The views management service.</param>
        public ViewsController(IViewManagementService viewService)
        {
            _viewService = viewService;
        }

        /// <summary>
        /// Retrieves all views.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Views
        /// View model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>Revisited</b>: bool?</li>
        /// </ul>
        /// </remarks>
        /// <returns>A list of views.</returns>
        /// <response code="200">Returns the list of views.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(GetViewsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() => Ok(await _viewService.GetViews());
        //...
        [HttpGet("{discussion_id}/{user_id}")]
        [ProducesResponseType(typeof(GetViewByIdsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int discussion_id, int user_id) => Ok(await _viewService.GetViewByIds(new(discussion_id, user_id)));

        /// <summary>
        /// Creates a new view.
        /// </summary>
        /// <remarks>
        /// Parameters <b>id</b> and <b>revisited</b> can be null. 
        ///  
        /// The <b>discussionId</b> and <b>userId</b> pair must be unique
        /// 
        /// Sample request:
        ///
        ///     POST api/Views
        ///     {
        ///       "id": null,
        ///       "source": "Google Chrome",
        ///       "discussionId": 4,
        ///       "userId": 6,
        ///       "revisited": null
        ///     }
        /// View model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>Revisited</b>: bool?</li>
        /// </ul>
        /// </remarks>
        /// <param name="view">The view to create.</param>
        /// <returns>The created view.</returns>
        /// <response code="200">Returns the created view.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreateViewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ViewModel view) => Ok(await _viewService.CreateView(new(view)));

        /// <summary>
        /// Deletes a view by ID.
        /// </summary>
        /// <remarks>
        /// Parameter <b>id</b> cannot be null. 
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Views/45
        /// View model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>Revisited</b>: bool?</li>
        /// </ul>
        /// </remarks>
        /// <param name="id">The ID of the view to delete.</param>
        /// <returns>A result indicating whether the deletion was successful.</returns>
        /// <response code="200">If the view was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteViewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id) => Ok(await _viewService.DeleteView(new(id)));

        /// <summary>
        /// Updates an existing view.
        /// </summary>
        /// <remarks>
        /// Parameter <b>revisited</b> can be null. 
        ///  
        /// The <b>discussionId</b> and <b>userId</b> pair must be unique
        /// 
        /// Any values different than those of the chosen view by <b>id</b> will replace the original.
        /// 
        /// Sample request:
        ///
        ///     PUT api/Views
        ///     {
        ///       "id": 8,
        ///       "source": "Google Chrome",
        ///       "discussionId": 4,
        ///       "userId": 6,
        ///       "revisited": true
        ///     }
        /// View model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Source</b>: string?</li>
        ///     <li><b>DiscussionId</b>: int</li>
        ///     <li><b>UserId</b>: int</li>
        ///     <li><b>Revisited</b>: bool?</li>
        /// </ul>
        /// </remarks>
        /// <param name="view">The view to update.</param>
        /// <returns>The updated view.</returns>
        /// <response code="200">Returns the updated view.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateViewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] ViewModel view) => Ok(await _viewService.UpdateView(new(view)));
    }
}
