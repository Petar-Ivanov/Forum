namespace ApplicationServices.Interfaces
{
    public interface IJWTAuthenticationManager
    {
        //string? Authenticate(string clientId, string secret);
        Task<string?> AuthenticateAsync(string username, string password);
    }
}
