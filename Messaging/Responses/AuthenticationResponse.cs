namespace Messaging.Responses
{
    /// <summary>
    /// Authentication response object
    /// </summary>
    public class AuthenticationResponse : ServiceResponseBase
    {
        public string Token { get; set; }

        public AuthenticationResponse(string token)
        {
            Token = token;
        }
    }
}
