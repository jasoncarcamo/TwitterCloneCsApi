using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TwitterCloneCs.Models;
using TwitterCloneCs.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;


namespace TwitterCloneCs.Services
{
    public class UserService
    {

        public UserService( IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string createToken(User user)
        {
            SymmetricSecurityKey secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetConnectionString("jwtSecret")));

            var hand = new JwtSecurityTokenHandler();

            Console.WriteLine(user);

            var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim("authorize", "true")
                });

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = "https://localhost:5001/",
                Audience = "https://localhost:5001/",
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddYears(12),
                SigningCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha512Digest)
            };

            var plainToken = hand.CreateToken(securityTokenDescriptor);
            var token = hand.WriteToken(plainToken);

            return token;
        }

        public string hashPassword(User user, string password)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            string hashedPassword = passwordHasher.HashPassword(user, password);

            return hashedPassword;
        }

        public bool comparePassword( User user, string password, string hashedPassword)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            PasswordVerificationResult matches = passwordHasher.VerifyHashedPassword(user, hashedPassword, password);

            if(matches == 0)
            {
                return false;
            }

            return true;

        }

    }
}
