using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParrotWings.Models.Domain.Transactions;
using ParrotWings.Models.OptionalParametres;
using ParrotWings.Services.Core.Extensions;
using ParrotWings.Services.Transactions;
using ParrotWings.WebAPI.ViewModels.Errors;
using ParrotWings.WebAPI.ViewModels.Transactions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ParrotWings.Services.Users;

namespace ParrotWings.WebAPI.Controllers.v1_00
{
    [Produces("application/json")]
    [Route("api/v1.00/Transactions")]
    [Authorize]
    public class TransactionsController : Controller
    {
        private IMapper mapper;
        private ITransactionService transactionService;
        private IUserService userService;

        public TransactionsController(IMapper mapper, ITransactionService transactionService, IUserService userService)
        {
            this.mapper = mapper;
            this.transactionService = transactionService;
            this.userService = userService;
        }

        // GET: api/transactions
        /// <summary>
        /// Get all transactions for current user
        /// </summary>
        /// <param name="paging">skip, take</param>
        /// <param name="filter">orderby, mode, party, search</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(Paging paging, TransactionsQuery filter)
        {
            var transactions = await transactionService.GetTransactionsAsync(User.GetUserId(), paging, filter);

            if (transactions == null)
                return StatusCode(204, new ErrorResponse { StatusCode = 204, Message = "Not found" });

            var result = mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(transactions) as List<TransactionViewModel>;

            result.ForEach(x =>
            {
                x.Type = x.Sender.Email == User.Identity.Name ? TransactionType.Credit : TransactionType.Debit;
                x.Correspondent = x.Sender.Email == User.Identity.Name ? x.Recipient : x.Sender;
            });

            return Ok(result);
        }

        // GET: api/transactions/5
        /// <summary>
        /// Get transaction by id (for current user)
        /// </summary>
        /// <param name="id">The transaction ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var transaction = await transactionService.FindByIdAsync(User.GetUserId(), id);

            if (transaction == null)
                return StatusCode(204, new ErrorResponse { StatusCode = 204, Message = "Not found" });

            var result = mapper.Map<Transaction, TransactionDetailsViewModel>(transaction);

            return Ok(result);
        }

        // GET: api/transactions/balance
        /// <summary>
        /// Get sum transactions debit/credit (for current user)
        /// </summary>
        /// <returns></returns>
        [HttpGet("balance")]
        public async Task<decimal> GetBalance()
        {
            return await transactionService.GetUserBalanceAsync(User.GetUserId());
        }

        // POST: api/transactions
        /// <summary>
        /// Send transaction
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TransactionPostModel postModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse { StatusCode = 400, Message = "Uncorrect data" });

            var user = await userService.FindUserByEmailOrNameAsync(postModel.Recipient.Email, postModel.Recipient.Name);

            if (user == null)
                return BadRequest(new ErrorResponse { StatusCode = 400, Message = "This user is not registered" });

            var transaction = mapper.Map<TransactionPostModel, Transaction>(postModel);

            try
            {
                await transactionService.SendTransactionAsync(User.GetUserId(), transaction, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { StatusCode = 400, Message = ex.Message });
            }

            return Ok();
        }
    }
}
