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

namespace ParrotWings.WebAPI.Controllers.v1_00
{
    [Produces("application/json")]
    [Route("api/v1.00/Transactions")]
    [Authorize]
    public class TransactionsController : Controller
    {
        private IMapper mapper;
        private ITransactionService transactionService;

        public TransactionsController(IMapper mapper, ITransactionService transactionService)
        {
            this.mapper = mapper;
            this.transactionService = transactionService;
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<IActionResult> Get(Paging paging, TransactionsQuery filter)
        {
            var transactions = await transactionService.GetTransactionsAsync(User.GetUserId(), paging, filter);

            if (transactions == null)
                return StatusCode(204, new ErrorResponse { StatusCode = 204, Message = "Not found"});

            var result = mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(transactions);

            return Ok(result);
        }

        // GET: api/transactions/5
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
        [HttpGet("balance")]
        public async Task<decimal> GetBalance()
        {
            return await transactionService.GetUserBalanceAsync(User.GetUserId());
        }

        // POST: api/transactions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TransactionPostModel postModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse { StatusCode = 400, Message = "Uncorrect data" });

            var transaction = mapper.Map<TransactionPostModel, Transaction>(postModel);

            try
            {
                await transactionService.SendTransactionAsync(User.GetUserId(), transaction, postModel.RecipientName);
            }
            catch(Exception ex)
            {
                return BadRequest(new ErrorResponse { StatusCode = 400, Message = ex.Message});
            }

            return Ok();
        }
    }
}
