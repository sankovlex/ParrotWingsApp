using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using ParrotWings.Models.Domain.Accounts;
using ParrotWings.Models.Options;
using ParrotWings.Models.Secure;
using ParrotWings.Services.Account;
using ParrotWings.WebAPI.Controllers.v1_00;
using ParrotWings.WebAPI.ViewModels.Account;
using ParrotWings.WebAPI.ViewModels.Errors;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParrotWings.Tests.API
{
    public class AccountControllerTest
    {
        private AccountController controller;

        private Mock<IAuthService> authService;
        private Mock<IOptions<AppSettings>> appSettings;
        private Mock<IMapper> mapper;

        public AccountControllerTest()
        {
            authService = new Mock<IAuthService>();
            appSettings = new Mock<IOptions<AppSettings>>();
            mapper = new Mock<IMapper>();

            controller = new AccountController(appSettings.Object, mapper.Object, authService.Object);
        }

        [Fact]
        public async Task Register_Ok()
        {
            //Arrage
            authService.Setup(x => x.CreateUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Act
            var action = await controller.Register(It.IsAny<UserRegisterPostModel>());

            //Assert
            var result = Assert.IsType<OkResult>(action);
            Assert.Equal(result.StatusCode, 200);
            authService.Verify();
        }

        [Fact]
        public async Task Register_ModelInvalid()
        {
            //Arrage & Act
            controller.ModelState.AddModelError("error", "some error");
            var action = await controller.Register(It.IsAny<UserRegisterPostModel>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(action);
        }

        [Fact]
        public async Task Register_Creating_Error()
        {
            //Arrage
            authService.Setup(x => x.CreateUserAsync(It.IsAny<User>()))
                .Returns(Task.FromException(new Exception()))
                .Verifiable();

            //Act
            var action = await controller.Register(It.IsAny<UserRegisterPostModel>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(action);
        }

        [Fact]
        public async Task Token_Ok()
        {
            //Arrage
            var token = new BearerToken() {
                Access_Token = "1234token",
                User_Email = "sankovlex@gmail.com"
            };

            var claims = new[] {
                new Claim(ClaimsIdentity.DefaultNameClaimType, "sankovlex@gmail.com")
            };

            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, null);

            authService.Setup(x => x.GetIdentityAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(identity));

            authService.Setup(x => x.GetBearerToken(identity))
                .Returns(token);

            //Act
            var action = await controller.Token(new UserLoginPostModel { Password = "1234", Email = "sankovlex@gmail.com"});

            //Assert
            var result = Assert.IsType<OkObjectResult>(action);
            Assert.Equal(token.Access_Token, "1234token");
        }

        [Fact]
        public async Task Token_ModelInvalid()
        {
            //Arrage & Act
            controller.ModelState.AddModelError("error", "some error");
            var action = await controller.Token(It.IsAny<UserLoginPostModel>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(action);
        }

        [Fact]
        public async Task Token_User_UnknownLoginOrPassword()
        {
            //Arrage
            authService.Setup(x => x.GetIdentityAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as ClaimsIdentity));

            //Act
            var action = await controller.Token(new UserLoginPostModel { Email = "1234", Password = "asdasd" });

            //Assert
            var result = Assert.IsType<BadRequestObjectResult>(action);
            Assert.Equal((result.Value as ErrorResponse).Message, "Unknown login or password");
        }
    }
}
