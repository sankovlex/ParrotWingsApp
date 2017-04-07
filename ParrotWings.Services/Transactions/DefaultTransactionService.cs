using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ParrotWings.Models.Domain.Transactions;
using ParrotWings.Models.OptionalParametres;
using ParrotWings.Data.Core.Repository;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using ParrotWings.Data.Extensions;
using ParrotWings.Services.Core;
using ParrotWings.Models.Domain.Accounts;

namespace ParrotWings.Services.Transactions
{
    public class DefaultTransactionService : ITransactionService
    {
        private IRepository<Transaction> transactionRepository;
        private IRepository<User> userRepository;

        public DefaultTransactionService(IRepository<Transaction> transactionRepository, IRepository<User> userRepository)
        {
            this.transactionRepository = transactionRepository;
            this.userRepository = userRepository;
        }

        public Transaction FindById(Guid userId, int transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Transaction> FindByIdAsync(Guid userId, int transactionId)
        {
            var transaction = await transactionRepository.FindAsync(
                x => x.TransactionId == transactionId,
                query => query.IncludeData(x => x.Recipient)
                    .IncludeData(x => x.Sender));

            if (transaction == null)
                return null;

            return transaction.SenderId == userId || transaction.RecipientId == userId
                ? transaction
                : null;
        }

        public IEnumerable<Transaction> GetTransactions(Guid id, Paging paging, TransactionsQuery filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid id, Paging paging, TransactionsQuery filter)
        {
            Func<IQueryable<Transaction>, IQueryable<Transaction>> query = q =>
            {
                //OrderBy
                if (string.IsNullOrEmpty(filter.OrderBy))
                    q = q.OrderByDescending(x => x.PropValue<Transaction>("datecreate"));
                else
                    q = q.OrderBy(x => x.PropValue<Transaction>(filter.OrderBy));

                q = q.Where(x => x.SenderId == id || x.RecipientId == id);

                //Filter
                if (filter.Mode == Mode.Sent)
                    q = q.Where(x => x.SenderId == id);
                else if (filter.Mode == Mode.Received)
                    q = q.Where(x => x.RecipientId == id);

                //Paging
                q = q.Skip(paging.Skip).Take(paging.Take);

                //Include
                q = q.IncludeData(x => x.Recipient).IncludeData(x => x.Sender);

                return q;
            };

            return await transactionRepository.GetAsync(query);
        }

        public decimal GetUserBalance(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> GetUserBalanceAsync(Guid id)
        {
            return await transactionRepository.GetSumAsync<decimal>(x => x.SenderId == id || x.RecipientId == id
            ? x.SenderId == id
                ? x.Amount * -1
                : x.Amount
            : 0);
        }

        public void SendTransaction(Guid id, Transaction transaction, string recipientName)
        {
            throw new NotImplementedException();
        }

        public async Task SendTransactionAsync(Guid id, Transaction transaction, string recipientName)
        {
            var balance = await GetUserBalanceAsync(id);

            if (balance < transaction.Amount)
                throw new Exception("Insufficient funds");

            transaction.SenderId = id;

            var recipient = await userRepository.FindAsync(x => x.Name == recipientName);

            transaction.Recipient = recipient ?? throw new Exception("No recipient specified");

            if (transaction.SenderId == recipient.UserId)
                throw new Exception("You can not send to yourself");

            await transactionRepository.CreateAsync(transaction);
        }
    }
}
