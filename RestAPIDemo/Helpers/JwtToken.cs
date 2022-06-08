using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RestAPIDemo.Helpers
{
    public static class JwtToken
    {
        private const string SECRET_KEY = "thisisabigsecretkeybubuyukgizlibiranahtardir";
        public static readonly SymmetricSecurityKey SIGNING_KEY =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        public static string GenerateJwtToken()
        {
            //var credentials = new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256);
            var credentials = new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256);

            // create a token
            var header = new JwtHeader(credentials);

            // token 1 dakika çalışacak
            DateTime Expiry = DateTime.UtcNow.AddMinutes(60);
            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

            var payload = new JwtPayload
            {
                { "sub", "subtest"},
                { "Name", "Yasin" },
                { "email", "yasintest@yasin.com" },
                { "exp", ts},
                { "iss", "https://localhost:5001"},
                { "aud", "https://localhost:5001"}
            };

            var securityToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(securityToken); 

            Console.WriteLine(tokenString);
            Console.WriteLine("Token Alındı!");
            return tokenString;
        }
    }
}
