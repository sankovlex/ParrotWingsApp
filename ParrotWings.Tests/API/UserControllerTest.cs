using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParrotWings.Models.Domain.Accounts;
using ParrotWings.Services.Users;
using ParrotWings.WebAPI.Controllers.v1_00;
using ParrotWings.WebAPI.MappingProfiles;
using ParrotWings.WebAPI.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParrotWings.Tests.API
{
    public class UserControllerTest
    {
        private Mock<IUserService> userService;
        private IMapper mapper;
        private UserController controller;

        private ClaimsPrincipal userPrincipal;
        private User testUser;

        public UserControllerTest()
        {
            //create mocks
            this.userService = new Mock<IUserService>();

            //configurate mapper (simplify testing, without mock)
            var mapperConfiguration = new MapperConfiguration(option =>
            {
                option.AddProfile<DomainToViewProfile>();
                option.AddProfile<ViewToDomainProfile>();
            });

            mapper = mapperConfiguration.CreateMapper();

            //init controller
            this.controller = new UserController(mapper, userService.Object);

            //create user identity
            var guid = Guid.NewGuid();

            var user = new User
            {
                Name = "Test",
                Email = "test@gmail.com",
                DateCreate = DateTime.Now,
                UserId = guid
            };

            var claims = new[] {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, null);

            userPrincipal = new ClaimsPrincipal(identity);
            testUser = user;
        }

        [Fact]
        public async Task Get_Ok()
        {
            //arrage
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userPrincipal }
            };

            userService.Setup(x => x.FindUserByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(testUser));
            
            //act
            var action = await controller.Get();

            //assert
            var result = Assert.IsType<OkObjectResult>(action);
            var model = Assert.IsType<UserProfileViewModel>(result.Value);
            Assert.Equal(model.Name, testUser.Name);
        }

        [Fact]
        public async Task Get_User_Not_Found()
        {
            //arrage
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userPrincipal }
            };

            userService.Setup(x => x.FindUserByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(null as User));

            //act
            var action = await controller.Get();

            //assert
            var result = Assert.IsType<StatusCodeResult>(action);
            Assert.Equal(result.StatusCode, 401);
        }

        [Fact]
        public async Task Search_Ok()
        {
            //arrage
            var users = new[] {
                new User { Name = "test1", Email = "t1@mail.com" },
                new User { Name = "test2", Email = "t2@mail.com" }
            }.AsEnumerable();

            userService.Setup(x => x.GetUsersByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(users));

            //act
            var action = await controller.Search("test");

            //assert
            var result = Assert.IsType<OkObjectResult>(action);
            var model = Assert.IsType<List<UserViewModel>>(result.Value);
            Assert.Equal(model.First().Name, users.First().Name);
        }

        [Fact]
        public async Task Search_NullFilter()
        {
            //act
            var action = await controller.Search(null);

            //assert
            var result = Assert.IsType<ObjectResult>(action);
            Assert.Equal(result.StatusCode, 204);
        }

        [Fact]
        public async Task Search_Empty()
        {
            userService.Setup(x => x.GetUsersByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(null as IEnumerable<User>));

            //act
            var action = await controller.Search(It.IsAny<string>());
            
            //assert
            var result = Assert.IsType<ObjectResult>(action);
            Assert.Equal(result.StatusCode, 204);
        }
    }
}
