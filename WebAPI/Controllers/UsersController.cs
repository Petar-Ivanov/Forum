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
    /// Controller for managing users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManagementService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The users management service.</param>
        public UsersController(IUserManagementService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Users
        /// User model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Username</b>: string</li>
        ///     <li><b>Password</b>: string</li>
        ///     <li><b>Email</b>: string</li>
        ///     <li><b>Country</b>: string</li>
        ///     <li><b>Biography</b>: string</li>
        ///     <li><b>BirthDay</b>: DateTime</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <returns>A list of users.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(GetUsersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() => Ok(await _userService.GetUsers());
        //...
        [HttpGet("{username}/{password}")]
        [ProducesResponseType(typeof(GetUserByUsernamePasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string username, string password) => Ok(await _userService.GetUserByUsernamePassword(new(username, password)));
        //...
        [HttpGet("username/{username}")]
        [ProducesResponseType(typeof(GetUsersByUsernameResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string username) => Ok(await _userService.GetUsersByUsername(new(username)));
        //...
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetUserByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id) => Ok(await _userService.GetUserById(new(id)));

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <remarks>
        /// Parameters <b>id</b>, <b>isVisible</b> and <b>updatedBy</b> can be null. 
        ///  
        /// The <b>username</b> parameter must be unique
        /// 
        /// Sample request:
        ///
        ///     POST api/Users
        ///     {
        ///         "id": null,
        ///         "username": "SampleName",
        ///         "password": "pass123",
        ///         "email": "mail@gmail.com",
        ///         "country": "Bulgaria",
        ///         "biography": "A short bio of the user in which they can share more of their background information",
        ///         "birthDay": "2024-05-18T22:36:40.973Z",
        ///         "isVisible": true,
        ///         "updatedBy": null
        ///     }
        /// User model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Username</b>: string</li>
        ///     <li><b>Password</b>: string</li>
        ///     <li><b>Email</b>: string</li>
        ///     <li><b>Country</b>: string</li>
        ///     <li><b>Biography</b>: string</li>
        ///     <li><b>BirthDay</b>: DateTime</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="user">The user to create.</param>
        /// <returns>The created user.</returns>
        /// <response code="200">Returns the created user.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] UserModel user) => Ok(await _userService.CreateUser(new(user)));

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <remarks>
        /// Parameter <b>id</b> cannot be null. 
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Users/45
        /// User model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Username</b>: string</li>
        ///     <li><b>Password</b>: string</li>
        ///     <li><b>Email</b>: string</li>
        ///     <li><b>Country</b>: string</li>
        ///     <li><b>Biography</b>: string</li>
        ///     <li><b>BirthDay</b>: DateTime</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A result indicating whether the deletion was successful.</returns>
        /// <response code="200">If the user was successfully deleted.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id) => Ok(await _userService.DeleteUser(new(id)));

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <remarks>
        /// Parameter <b>isVisible</b> can be null. 
        ///  
        /// The <b>username</b> parameter must be unique
        /// 
        /// Any values different than those of the chosen user by <b>id</b> will replace the original.
        /// 
        /// Sample request:
        ///
        ///     PUT api/Users
        ///     {
        ///         "id": 4,
        ///         "username": "SampleName",
        ///         "password": "newpass123",
        ///         "email": "mail@gmail.com",
        ///         "country": "Bulgaria",
        ///         "biography": "A short bio of the user in which they can share more of their background information",
        ///         "birthDay": "2024-05-18T22:36:40.973Z",
        ///         "isVisible": true,
        ///         "updatedBy": 9
        ///     }
        /// User model: 
        /// <ul>
        ///     <li><b>Id</b>: int?</li>
        ///     <li><b>Username</b>: string</li>
        ///     <li><b>Password</b>: string</li>
        ///     <li><b>Email</b>: string</li>
        ///     <li><b>Country</b>: string</li>
        ///     <li><b>Biography</b>: string</li>
        ///     <li><b>BirthDay</b>: DateTime</li>
        ///     <li><b>IsVisible</b>: bool?</li>
        ///     <li><b>UpdatedBy</b>: int?</li>
        /// </ul>
        /// </remarks>
        /// <param name="user">The user to update.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] UserModel user) => Ok(await _userService.UpdateUser(new(user)));
    }
}
