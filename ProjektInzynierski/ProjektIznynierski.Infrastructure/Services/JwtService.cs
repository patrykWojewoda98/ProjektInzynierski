using Microsoft.IdentityModel.Tokens;

using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjektIznynierski.Infrastructure.Services
{
    public class JwtService : IJwtTokenService
    {
        private readonly string _secretKey;
        private readonly int _expiryDuration;
        private readonly string _issuer;
        private readonly string _audience;
        public JwtService()
        {
            _secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                ?? throw new InvalidOperationException("JWT_SECRET_KEY environment variable is not set.");

            _expiryDuration = int.Parse(
                Environment.GetEnvironmentVariable("JWT_EXPIRY_DURATION")
                ?? throw new InvalidOperationException("JWT_EXPIRY_DURATION environment variable is not set.")
            );

            _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? throw new InvalidOperationException("JWT_ISSUER environment variable is not set.");

            _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                ?? throw new InvalidOperationException("JWT_AUDIENCE environment variable is not set.");
        }


        public Task<string> GenerateToken(Client client)
        {
            var claims = new List<Claim>
            {
                new Claim("id", client.Id.ToString()),
                new Claim("name", client.Name )
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expiryDuration),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
