using ApplicationServices.Interfaces;
using Messaging.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Authentication controller.
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IJWTAuthenticationManager _jwtAuthenticationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationController"/> class.
        /// </summary>
        /// <param name="jWTAuthenticationManager">Jwt authentication manager</param>
        public AuthorizationController(IJWTAuthenticationManager jWTAuthenticationManager)
        {
            _jwtAuthenticationManager = jWTAuthenticationManager;
        }

        /// <summary>
        /// Generate a jwt token.
        /// </summary>
        /// <param name="username">Client username</param>
        /// <param name="password">Client password</param>
        /// <returns></returns>
        //[HttpGet]
        //[HttpGet("token/{username}/{password}")]
        //public async Task<IActionResult> Token(string username, string password)
        //{
        //    string? token = _jwtAuthenticationManager.Authenticate(username, password);
        //    ArgumentNullException.ThrowIfNull(token);

        //    return Ok(await Task.FromResult(new AuthenticationResponse(token)));
        //}

        //[HttpGet("token/{username}/{password}")]
        //public IActionResult Token(string username, string password)
        //{
        //    string? token = _jwtAuthenticationManager.Authenticate(username, password);

        //    if (token == null)
        //    {
        //        return Unauthorized(new { Message = "Invalid credentials" });
        //    }

        //    return Ok(new AuthenticationResponse(token));
        //}
        [HttpGet("token/{username}/{password}")]
        public async Task<IActionResult> Token(string username, string password)
        {
            string? token = await _jwtAuthenticationManager.AuthenticateAsync(username, password);

            if (token == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            return Ok(new AuthenticationResponse(token));
        }
    }
}
