using ParrotWings.WebAPI.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParrotWings.WebAPI.ViewModels.Transactions
{
    public class TransactionDetailsViewModel
    {
        public long TransactionId { get; set; }

        public DateTime DateCreate { get; set; }

        public decimal Amount { get; set; }

        public string Message { get; set; }

        public UserViewModel Sender { get; set; }

        public UserViewModel Recipient { get; set; }
    }
}
