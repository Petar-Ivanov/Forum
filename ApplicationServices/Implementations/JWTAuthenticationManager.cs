using ApplicationServices.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Implementations
{
    public class JWTAuthenticationManager: IJWTAuthenticationManager
    {

        private readonly IUnitOfWork _unitOfWork;
        //private readonly IUsersRepository _users;

        private readonly string _tokenKey;

        public JWTAuthenticationManager(string _tokenKey, IUnitOfWork unitOfWork)
        {
            this._tokenKey = _tokenKey;
            _unitOfWork = unitOfWork;
        }

        
        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            if (!users.Any(x => x.Username == username && x.Password == password))
            {
                return null;
            }

            JwtSecurityTokenHandler handler = new();
            var key = Encoding.ASCII.GetBytes(_tokenKey);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new(new Claim[]
                {
            new(ClaimTypes.Name, username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}
