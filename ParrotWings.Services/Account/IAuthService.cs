using Microsoft.IdentityModel.Tokens;
using ParrotWings.Models.Domain.Accounts;
using ParrotWings.Models.Secure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParrotWings.Services.Account
{
    public interface IAuthService
    {
        /// <summary>
        /// Add a user to database
        /// </summary>
        /// <param name="user">User data</param>
        void CreateUser(User user);
        /// <summary>
        /// ASYNC: Add a user to database
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns></returns>
        Task CreateUserAsync(User user);

        /// <summary>
        /// Get user identity
        /// </summary>
        /// <param name="username">Principal</param>
        /// <returns></returns>
        ClaimsIdentity GetIdentity(string username, string password);
        /// <summary>
        /// ASYNC: Get user identity
        /// </summary>
        /// <param name="username">Principal</param>
        /// <returns></returns>
        Task<ClaimsIdentity> GetIdentityAsync(string username, string password);

        /// <summary>
        /// Get bearer security token
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        BearerToken GetBearerToken(ClaimsIdentity identity);
    }
}
