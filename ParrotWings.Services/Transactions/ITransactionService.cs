using ParrotWings.Models.Domain.Transactions;
using ParrotWings.Models.OptionalParametres;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParrotWings.Services.Transactions
{
    public interface ITransactionService
    {
        decimal GetUserBalance(Guid id);
        Task<decimal> GetUserBalanceAsync(Guid id);

        IEnumerable<Transaction> GetTransactions(Guid id, Paging paging, TransactionsQuery filter);
        Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid id, Paging paging, TransactionsQuery filter);

        Transaction FindById(Guid userId, int transactionId);
        Task<Transaction> FindByIdAsync(Guid userId, int transactionId);

        void SendTransaction(Guid id, Transaction transaction, string recipientName);
        Task SendTransactionAsync(Guid id, Transaction transaction, string recipientName);
    }
}
