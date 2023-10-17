using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models_Layer.ModelRequest;
using Services_Layer.ServiceInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services_Layer.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string BuildToken(string key, string issuer, UserRequest user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var claims = new ClaimsIdentity(new Claim[] {
                                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    new Claim(ClaimTypes.Actor, user.UserName),
                                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                                    new Claim(ClaimTypes.Role, user.Role),
                                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            });

            var signIn = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256);
            var tokenDescription = new SecurityTokenDescriptor 
            {
                Subject = claims,
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = signIn
            };                

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return AccessToken;
        }
        public bool IsTokenValid(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

}
