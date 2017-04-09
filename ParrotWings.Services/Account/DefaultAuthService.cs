using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ParrotWings.Models.Domain.Accounts;
using Microsoft.AspNetCore.Http;
using ParrotWings.Data.Core.Repository;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ParrotWings.Services.Secure;
using ParrotWings.Models.Secure;
using Microsoft.Extensions.Options;
using ParrotWings.Models.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using ParrotWings.Models.Domain.Transactions;

namespace ParrotWings.Services.Account
{
    public class DefaultAuthService : IAuthService
    {
        private AppSettings appSettings;
        private IPasswordProtector passwordProtector;
        private IRepository<User> userRepository;
        private IRepository<Transaction> transactionRepository;

        private const string AUTH_NAME = "Bearer";

        public DefaultAuthService(IOptions<AppSettings> appSettings, IRepository<User> userRepository, IPasswordProtector passwordProtector, IRepository<Transaction> transactionRepository)
        {
            this.appSettings = appSettings.Value;
            this.userRepository = userRepository;
            this.transactionRepository = transactionRepository;
            this.passwordProtector = passwordProtector;
        }

        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task CreateUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Password))
                throw new ArgumentNullException();

            var protect = passwordProtector.Protect(user.Password);

            user.Password = protect.hash;
            user.Salt = protect.salt;

            try
            {
                await userRepository.CreateAsync(user);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 547 || ex.Number == 2601)
                    throw new Exception("A user with this email is registered");
                throw new Exception($"Code: {ex.Number}. {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception("A user with this email is registered");
            }

            var pw = await userRepository.FindAsync(x => x.Email == "parrot@wings.app");
            var newUser = await userRepository.FindAsync(x => x.Email == user.Email);

            await transactionRepository.CreateAsync(new Transaction {
                Amount = 500,
                Message = "Welcome to our service!",
                Sender = pw,
                Recipient = newUser
            });
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = userRepository.Find(x => x.Email == username);

            if (user == null)
                return null;

            if (!passwordProtector.Verify(password, user.Password, user.Salt))
                return null;

            var claims = new[] {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
            };

            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, null);

            return identity;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(string username, string password)
        {
            var user = await userRepository.FindAsync(x => x.Email == username);

            if (user == null)
                return null;

            if (!passwordProtector.Verify(password, user.Password, user.Salt))
                return null;

            var claims = new[] {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, null);

            return identity;
        }

        public BearerToken GetBearerToken(ClaimsIdentity identity)
        {
            var jwtToken = new JwtSecurityToken(
                issuer: appSettings.Issuer,
                audience: appSettings.Audience,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(3)),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    key: new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.AppId)),
                    algorithm: SecurityAlgorithms.HmacSha256));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new BearerToken()
            {
                Access_token = accessToken,
                User_email = identity.Name
            };
        }
    }
}
