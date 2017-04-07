using ParrotWings.Models.Domain.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParrotWings.WebAPI.ViewModels.Account
{
    public class UserProfileViewModel
    {
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public string Email { get; set; }
    }
}
