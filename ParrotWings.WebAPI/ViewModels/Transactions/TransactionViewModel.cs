using ParrotWings.WebAPI.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParrotWings.WebAPI.ViewModels.Transactions
{
    public class TransactionViewModel
    {
        public long TransactionId { get; set; }

        public string DateCreate { get; set; }

        public string Message { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public UserViewModel Sender { get; set; }
        
        public UserViewModel Recipient { get; set; }

        public UserViewModel Correspondent { get; set; }
    }

    public enum TransactionType
    {
        Debit,
        Credit
    }
}
