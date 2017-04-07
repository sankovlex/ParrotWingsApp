using ParrotWings.Models.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWings.Models.Domain.Transactions
{
    public class Transaction
    {
        public long TransactionId { get; set; }

        public DateTime DateCreate { get; set; }

        public decimal Amount { get; set; }

        public string Message { get; set; }

        //relations
        public Guid SenderId { get; set; }
        public virtual User Sender { get; set; }

        public Guid RecipientId { get; set; }
        public virtual User Recipient { get; set; }
    }
}
