using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DailyApp.Token
{
    public class TokenGenerator
    {
        public string GenerateToken(List<Claim> userClaims)
        {



            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            //var tokenKey = Encoding.UTF8.GetBytes(_config.GetSection("Token:Key").Value);
            var tokenKey = Encoding.UTF8.GetBytes("GucluBirSifreTokenSifresi");

            SigningCredentials signingCredentials = new(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "http://localhost",
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: userClaims);

            return handler.WriteToken(token);

        }


    }
}

