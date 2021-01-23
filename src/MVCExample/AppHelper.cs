using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MVCExample
{
    public class AppHelper
    {
        /// <summary>
        /// New secret
        /// </summary>
        /// <returns></returns>
        public static string GetNewSecret()
        {
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[256 / 8];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// New token
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="issued"></param>
        /// <param name="audience"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GetNewToken(int channelId, string issued, string audience, string secret)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, channelId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, channelId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(365),
                Issuer = issued,
                Audience = audience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
