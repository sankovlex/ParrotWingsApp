using ParrotWings.WebAPI.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParrotWings.WebAPI.ViewModels.Transactions
{
    public class TransactionPostModel
    {
        public decimal Amount { get; set; }

        public string Message { get; set; }
        
        public string RecipientName { get; set; }
    }
}
