using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ParrotWings.Services.Transactions;
using ParrotWings.Services.Core.Extensions;
using ParrotWings.Models.OptionalParametres;
using AutoMapper;
using ParrotWings.Models.Domain.Transactions;
using ParrotWings.WebAPI.ViewModels.Transactions;
using ParrotWings.Services.Users;
using Account = ParrotWings.Models.Domain.Accounts;
using ParrotWings.WebAPI.ViewModels.Account;
using ParrotWings.WebAPI.ViewModels.Errors;

namespace ParrotWings.WebAPI.Controllers.v1_00
{
    [Produces("application/json")]
    [Route("api/v1.00/User")]
    [Authorize]
    public class UserController : Controller
    {
        private IMapper mapper;
        private IUserService userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await userService.FindUserByIdAsync(User.GetUserId());

            if (user == null)
                return StatusCode(401);

            var result = mapper.Map<Account.User, UserProfileViewModel>(user);

            return Ok(result);
        }

        // GET: api/user/search?filter={string}
        /// <summary>
        /// Search users by name
        /// </summary>
        /// OPTIONS
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<IActionResult> Search(string filter)
        {
            if (filter == null)
                return StatusCode(204, new ErrorResponse { StatusCode = 204, Message = "Not found" });

            var users = await userService.GetUsersByNameAsync(filter);

            if (users == null)
                return StatusCode(204, new ErrorResponse { StatusCode = 204, Message = "Not found" });

            var result = mapper.Map<IEnumerable<Account.User>, IEnumerable<UserViewModel>>(users);

            return Ok(result);
        }
    }
}
