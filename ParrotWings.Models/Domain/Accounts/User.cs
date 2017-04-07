using ParrotWings.Models.Domain.Transactions;
using System;
using System.Collections.Generic;

namespace ParrotWings.Models.Domain.Accounts
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }

        public DateTime DateCreate { get; set; }

        public virtual ICollection<Transaction> SentTransactions { get; set; }
        public virtual ICollection<Transaction> ReceivedTransations { get; set; }
    }
}
