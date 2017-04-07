using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParrotWings.Models.Domain.Accounts;
using ParrotWings.Models.Domain.Transactions;
using ParrotWings.Models.OptionalParametres;
using ParrotWings.Services.Transactions;
using ParrotWings.WebAPI.Controllers.v1_00;
using ParrotWings.WebAPI.MappingProfiles;
using ParrotWings.WebAPI.ViewModels.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParrotWings.Tests.API
{
    public class TransactionsControllerTest
    {
        private Mock<ITransactionService> transactionService;
        private IMapper mapper;
        private TransactionsController controller;

        public TransactionsControllerTest()
        {
            //create mocks
            this.transactionService = new Mock<ITransactionService>();

            //configurate mapper (simplify testing, mapper test isolated)
            var mapperConfiguration = new MapperConfiguration(option =>
            {
                option.AddProfile<DomainToViewProfile>();
                option.AddProfile<ViewToDomainProfile>();
            });

            mapper = mapperConfiguration.CreateMapper();

            //init controller
            this.controller = new TransactionsController(mapper, transactionService.Object);
        }

        private User CreateTestUser(string name, string email)
        {
            return new User
            {
                Name = name,
                Email = email,
                DateCreate = DateTime.Now,
                UserId = Guid.NewGuid()
            };
        }

        private ClaimsPrincipal CreateUserPrincipal(User user)
        {
            var claims = new[] {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, null);

            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public async Task Get_Transactions_Ok()
        {
            //arrage
            var currentUser = CreateTestUser("test 1", "test1@mail.com");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = CreateUserPrincipal(currentUser)
                }
            };

            var testUser1 = CreateTestUser("test 2", "test2@mail.com");
            var testUser2 = CreateTestUser("test 3", "test3@mail.com");

            var transactions = new[] {
                new Transaction{ DateCreate = DateTime.Now, Amount = 10,
                    Recipient = currentUser, Sender = testUser1},
                new Transaction{ DateCreate = DateTime.Now, Amount = 20,
                    Recipient = testUser1, Sender = currentUser},
                new Transaction{ DateCreate = DateTime.Now, Amount = 30,
                    Recipient = testUser2, Sender = currentUser},
            }.AsEnumerable();

            transactionService.Setup(x => x.GetTransactionsAsync(
                It.IsAny<Guid>(),
                It.IsAny<Paging>(),
                It.IsAny<TransactionsQuery>()))
                .Returns(Task.FromResult(transactions));

            //act
            var action = await controller.Get(It.IsAny<Paging>(), It.IsAny<TransactionsQuery>());

            //assert
            var result = Assert.IsType<OkObjectResult>(action);
            var model = Assert.IsType<List<TransactionViewModel>>(result.Value);
            Assert.False(!model.Any());
            Assert.Equal(transactions.First().Amount, model.First().Amount);
        }

        [Fact]
        public async Task Get_Transactions_Empty()
        {
            //arrage
            var currentUser = CreateTestUser("test 1", "test1@mail.com");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = CreateUserPrincipal(currentUser)
                }
            };
            
            transactionService.Setup(x => x.GetTransactionsAsync(
                It.IsAny<Guid>(),
                It.IsAny<Paging>(),
                It.IsAny<TransactionsQuery>()))
                .Returns(Task.FromResult(null as IEnumerable<Transaction>));

            //act
            var action = await controller.Get(It.IsAny<Paging>(), It.IsAny<TransactionsQuery>());

            //assert
            var result = Assert.IsType<ObjectResult>(action);
            Assert.Equal(result.StatusCode, 204);
        }
    }
}
