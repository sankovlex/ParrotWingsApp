using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParrotWings.Models.Options;
using ParrotWings.Services.Account;
using ParrotWings.Services.Core.Extensions;
using ParrotWings.WebAPI.ViewModels.Account;
using ParrotWings.WebAPI.ViewModels.Errors;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account = ParrotWings.Models.Domain.Accounts;

namespace ParrotWings.WebAPI.Controllers.v1_00
{
    [Produces("application/json")]
    [Route("api/v1.00/Account")]
    public class AccountController : Controller
    {
        private readonly AppSettings appSettings;
        private IMapper mapper;
        private IAuthService authService;

        public AccountController(IOptions<AppSettings> appSettings, IMapper mapper, IAuthService authService)
        {
            this.appSettings = appSettings.Value;

            this.mapper = mapper;

            this.authService = authService;
        }

        // POST: account/token
        /// <summary>
        /// Get bearer authentication token
        /// </summary>
        /// <param name="userPostModel"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody]UserLoginPostModel userPostModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Uncorrected data"
                });

            var user = await authService.GetIdentityAsync(
                username: userPostModel.Email,
                password: userPostModel.Password);

            if (user == null)
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Invalid   login or password"
                });
            
            var accessToken = authService.GetBearerToken(user);

            return Ok(accessToken);
        }

        // POST: account/register
        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="userPostModel"></param>
        /// <returns></returns>
        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserRegisterPostModel userPostModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Uncorrected data"
                });

            var user = mapper.Map<UserRegisterPostModel, Account.User>(userPostModel);

            try
            {
                await authService.CreateUserAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse {
                    Message = ex.Message,
                    StatusCode = 400
                });
            }

            return Ok();
        }
    }
}
