using DotNetCardsServer.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetCardsServer.Auth
{
    public class JwtHelper
    {
       public static readonly string secretKey = "F8C2E94F-694F-4888-B434-7B0B228239D4";

        public static string GenerateAuthToken(User user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new Claim[]
            {
                new Claim("id",user.Id.ToString()),
                new Claim("isAdmin",user.IsAdmin.ToString()),
                new Claim("isBusiness",user.IsBusiness.ToString()),
                new Claim("first",user.Name.First.ToString()),
                new Claim ("role",user.IsAdmin ? "Admin":user.IsBusiness ? "Business":"User")
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "CardsServer",
                audience: "CardReactFront",
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
