using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;

namespace api_donkey_air.Controllers
{
    public class AuthService
    {

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }

    public class JWTService
    {
        private readonly string? _secretKey;
        private readonly int _expireMinutes;

        public JWTService(IConfiguration configuration)
        {
            _secretKey = configuration["JwtSettings:SecretKey"];
            _expireMinutes = int.Parse(configuration["JwtSettings:ExpireMinutes"]);
        }

        public string GenerateToken(string username)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(_expireMinutes),
                signingCredentials: creds,
                claims:[ new Claim(ClaimTypes.Name, username)]);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
