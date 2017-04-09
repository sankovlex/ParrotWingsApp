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
                //Include
                q = q.IncludeData(x => x.Recipient).IncludeData(x => x.Sender);

                //OrderBy
                if (string.IsNullOrEmpty(filter.OrderBy))
                    q = q.OrderByDescending(x => x.PropValue<Transaction>("datecreate"));
                else if (filter.OrderBy.ToLower() == "name")
                    q = q.OrderBy(x => x.SenderId == id ? x.Recipient.Name : x.Sender.Name);
                else
                    q = q.OrderBy(x => x.PropValue<Transaction>(filter.OrderBy));


                q = q.Where(x => x.SenderId == id || x.RecipientId == id);

                //Filter
                if (filter.Mode == Mode.Sent)
                    q = q.Where(x => x.SenderId == id);
                else if (filter.Mode == Mode.Received)
                    q = q.Where(x => x.RecipientId == id);



                //this need sf/sp :(
                /*
                exec sp_executesql N'SELECT [x].[TransactionId], [x].[Amount], [x].[DateCreate], [x].[Message], [x].[RecipientId], [x].[SenderId], [u].[UserId], [u].[DateCreate], [u].[Email], [u].[Name], [u].[Password], [u].[Salt], [u0].[UserId], [u0].[DateCreate], [u0].[Email], [u0].[Name], [u0].[Password], [u0].[Salt]
                FROM [Transactions] AS [x]
                INNER JOIN [Users] AS [u] ON [x].[RecipientId] = [u].[UserId]
                INNER JOIN [Users] AS [u0] ON [x].[SenderId] = [u0].[UserId]
                WHERE ([x].[SenderId] = @__id_0) OR ([x].[RecipientId] = @__id_1)',N'@__id_0 uniqueidentifier,@__id_1 uniqueidentifier',@__id_0='0505AB75-941B-E711-9687-BCEE7B5B2BE5',@__id_1='0505AB75-941B-E711-9687-BCEE7B5B2BE5'
                go
                 */
                if (!string.IsNullOrEmpty(filter.Party))
                    q = q.Where(x => x.Recipient.Name.ToLower().Contains(filter.Party.ToLower()) || x.Sender.Name.ToLower().Contains(filter.Party.ToLower()));

                //very very low performance
                if (!string.IsNullOrEmpty(filter.Search))
                {
                    decimal? value = decimal.TryParse(filter.Search, out decimal tmp) ? tmp : (decimal?)null;
                    DateTime? date = DateTime.TryParse(filter.Search, out DateTime dTmp) ? dTmp : (DateTime?)null;

                    if (value.HasValue)
                        q = q.Where(x => x.Amount == value.Value);
                    else if (date.HasValue)
                        q = q.Where(x => x.DateCreate.CompareTo(date.Value) == 0);
                    else
                        q = q.Where(x => x.Recipient.Name.ToLower().Contains(filter.Search.ToLower()) || x.Sender.Name.ToLower().Contains(filter.Search.ToLower()));
                }

                //Paging
                q = q.Skip(paging.Skip).Take(paging.Take);

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

        public void SendTransaction(Guid id, Transaction transaction, User user)
        {
            throw new NotImplementedException();
        }

        public async Task SendTransactionAsync(Guid id, Transaction transaction, User user)
        {
            transaction.Recipient = user ?? throw new Exception("No recipient specified");

            var balance = await GetUserBalanceAsync(id);

            if (balance < transaction.Amount)
                throw new Exception("Insufficient funds");

            transaction.SenderId = id;

            if (transaction.SenderId == user.UserId)
                throw new Exception("You can not send to yourself");

            await transactionRepository.CreateAsync(transaction);
        }
    }
}
